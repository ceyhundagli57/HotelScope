using System.Text;
using System.Text.Json;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Application.Services;

public class ReportService: IReportService
{ 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRabbitMqConnectionManager _rabbitMqConnectionManager;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IRabbitMqConnectionManager rabbitMqConnectionManager,
            ILogger<ReportService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _rabbitMqConnectionManager = rabbitMqConnectionManager;
            _logger = logger;
        }

        /// <summary>
        /// Generates a report request and publishes it to RabbitMQ.
        /// </summary>
        public async Task<Guid> GenerateReportAsync(GenerateReportRequestDto request)
        {
            _logger.LogInformation("Generating report for location: {Location}", request.Location);

            // Create a new report entity
            var report = new ReportEntity
            {
                Id = Guid.NewGuid(),
                Location = string.Empty,
                RequestDate = DateTime.UtcNow,
                Status = ReportStatus.Preparing
            };

            // Save the report to the database
            await _unitOfWork.Report.AddAsync(report);
            await _unitOfWork.SaveAsync();

            // Publish the report request to RabbitMQ
            try
            {
                await PublishToQueueAsync(new SaveReportRequestDto(){Id = report.Id, Location = request.Location});
                _logger.LogInformation("Report request published to RabbitMQ: ReportId={ReportId}", report.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish report request to RabbitMQ: ReportId={ReportId}", report.Id);
                throw;
            }

            return report.Id;
        }

        /// <summary>
        /// Retrieves the status of a specific report.
        /// </summary>
        public async Task<ReportStatusDto> GetReportStatusAsync(Guid reportId)
        {
            _logger.LogInformation("Fetching status for report: {ReportId}", reportId);

            var report = await _unitOfWork.Report.GetAsync(reportId);
            if (report == null)
            {
                _logger.LogWarning("Report not found: {ReportId}", reportId);
                throw new KeyNotFoundException($"Report with ID {reportId} not found.");
            }

            return new ReportStatusDto
            {
                Id = report.Id,
                Status = report.Status
            };
        }

        /// <summary>
        /// Retrieves a specific report by its ID.
        /// </summary>
        public async Task<ReportEntity> GetReportByIdAsync(Guid reportId)
        {
            _logger.LogInformation("Fetching details for report: {ReportId}", reportId);

            var report = await _unitOfWork.Report.GetAsync(reportId);
            if (report == null)
            {
                _logger.LogWarning("Report not found: {ReportId}", reportId);
                throw new KeyNotFoundException($"Report with ID {reportId} not found.");
            }

            return report;
        }

        public async Task<List<ReportHotelDto>> GetAllHotelsByCity(string city)
        {
            return await _unitOfWork.GetAllHotelsByCity(city);
            
        }

        public async Task SaveReportAsync(ReportEntity reportEntity)
        {
             _unitOfWork.Report.Update(reportEntity);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Retrieves a summary of all reports.
        /// </summary>
        public async Task<IEnumerable<ReportSummaryDto>> GetAllReportsAsync()
        {
            _logger.LogInformation("Fetching all reports.");

            var reports = await _unitOfWork.Report.GetAllAsync();
            return _mapper.Map<IEnumerable<ReportSummaryDto>>(reports);
        }

        /// <summary>
        /// Publishes a report request message to RabbitMQ.
        /// </summary>
        private async Task PublishToQueueAsync(SaveReportRequestDto message)
        {
            try
            {
                using var connection = await _rabbitMqConnectionManager.GetConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: "report_requests",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

               
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                await channel.BasicPublishAsync(
                    exchange: "report_exchange",
                    routingKey: "report_requests",
                    body: body
                );
                _logger.LogInformation("Message published: ReportId={ReportId}, Location={Location}", message.Id, message.Location);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing message to RabbitMQ.");
                throw;
            }
        }
    
}