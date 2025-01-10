using System.Security.Authentication;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ReportWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRabbitMqConnectionManager _connectionManager;
    private IConnection _connection;
    private IChannel _channel;

    public Worker(ILogger<Worker> logger, IRabbitMqConnectionManager connectionManager)
    {
        _logger = logger;
        _connectionManager = connectionManager;
       InitRabbitMq();
    }

    private void InitRabbitMq()
    {
        _connection =  _connectionManager.GetConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;
        var queueDeclareOk = _channel.QueueDeclareAsync(
            queue: "report_requests",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null).Result;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
         try
         {
             var consumer = new AsyncEventingBasicConsumer(_channel);
             consumer.ReceivedAsync += async (model, ea) =>
             {
                 var body = ea.Body.ToArray();
                 var message = Encoding.UTF8.GetString(body);
                 Console.WriteLine($"Message received.{message}");
                 try
                 {
                     var requestModel = JsonConvert.DeserializeObject<SaveReportRequestDto>(message);
                     if (requestModel != null)
                     {
                         var saveReportRequestDto = new SaveReportRequestDto
                         {
                             Id = requestModel.Id,
                             Location = requestModel.Location
                         };

                        await SendCreateReportRequest(saveReportRequestDto);
                     }
                 }
                 catch (Exception e)
                 {
                     // ignored
                 }

                 _logger.LogInformation("Received message: {Message}", message);
             };

             await _channel.BasicConsumeAsync(
                 queue: "report_requests",
                 autoAck: true,
                 consumer: consumer, cancellationToken: stoppingToken);

         }
         catch (Exception e)
         {
             Console.WriteLine(e.Message);
         }
         while (!stoppingToken.IsCancellationRequested)
         {
             await Task.Delay(1000, stoppingToken);
         }

    }
    
    private async Task<bool> SendCreateReportRequest(SaveReportRequestDto saveReportRequestDto)
    {
        // Serialize the object to JSON
        string json = JsonSerializer.Serialize(saveReportRequestDto);

        // Prepare the HTTP client
        using HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiUrl"));
        // Set up the request content with JSON
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Send the POST request
        HttpResponseMessage response = await client.PostAsync("reports", content);

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