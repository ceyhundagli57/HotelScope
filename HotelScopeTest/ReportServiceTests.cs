using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelScopeTest;

public class ReportServiceTests
{
    
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRabbitMqConnectionManager> _rabbitMqConnectionManagerMock;
    private readonly Mock<ILogger<ReportService>> _loggerMock;
    private readonly ReportService _reportService;

    public ReportServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _rabbitMqConnectionManagerMock = new Mock<IRabbitMqConnectionManager>();
        _loggerMock = new Mock<ILogger<ReportService>>();

        _reportService = new ReportService(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _rabbitMqConnectionManagerMock.Object,
            _loggerMock.Object
        );
    }
    
    [Fact]
    public async Task GetReportStatusAsync_Should_ReturnReportStatus()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var report = new ReportEntity { Id = reportId, Status = ReportStatus.Preparing };

        _unitOfWorkMock.Setup(uow => uow.Report.GetAsync(reportId, null, false))
            .ReturnsAsync(report);

        // Act
        var result = await _reportService.GetReportStatusAsync(reportId);

        // Assert
        Assert.Equal(reportId, result.Id);
        Assert.Equal(ReportStatus.Preparing, result.Status);
    }

    [Fact]
    public async Task GetReportStatusAsync_Should_ThrowIfReportNotFound()
    {
        // Arrange
        var reportId = Guid.NewGuid();

        _unitOfWorkMock.Setup(uow => uow.Report.GetAsync(reportId, null, false))
            .ReturnsAsync((ReportEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _reportService.GetReportStatusAsync(reportId));
    }
}