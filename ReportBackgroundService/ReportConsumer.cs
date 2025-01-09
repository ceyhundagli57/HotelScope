using System.Text;
using System.Text.Json;
using Application.DTOs;
using Application.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace ReportBackgroundService;

public class ReportConsumer: BackgroundService
{
        private readonly IRabbitMqConnectionManager _connectionManager;
        private readonly ILogger<ReportConsumer> _logger;

        public ReportConsumer(IRabbitMqConnectionManager connectionManager, ILogger<ReportConsumer> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {

            using var connection = await _connectionManager.GetConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "report_requests",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var requestModel = JsonConvert.DeserializeObject<SaveReportRequestDto>(message);
                var saveReportRequestDto = new SaveReportRequestDto
                {
                    Id = requestModel.Id,
                    Location = requestModel.Location
                };

                await SendCreateReportRequest(saveReportRequestDto);
                _logger.LogInformation("Received message: {Message}", message);

                // Process the message (simulate long-running task)
                await Task.Delay(1000, cancellationToken);

                _logger.LogInformation("Processed message: {Message}", message);
            };

            await channel.BasicConsumeAsync(
                queue: "report_requests",
                autoAck: true,
                consumer: consumer
            );

            _logger.LogInformation("Consumer started. Press Ctrl+C to exit.");
            await Task.Delay(Timeout.Infinite, cancellationToken); // Keep the consumer running        }
        }

        private async Task<bool> SendCreateReportRequest(SaveReportRequestDto saveReportRequestDto)
        {
            // Serialize the object to JSON
            string json = JsonSerializer.Serialize(saveReportRequestDto);

            // Prepare the HTTP client
            using HttpClient client = new HttpClient();

            // Set up the request content with JSON
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request
            HttpResponseMessage response = await client.PostAsync("https://localhost:7006/reports", content);

            // Check the response
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Request successful!");
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {responseBody}");
                return true;
            }
            else
            {
                Console.WriteLine($"Request failed. Status code: {response.StatusCode}");
                return false;
            }
        }
    
}