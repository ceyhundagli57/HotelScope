using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IReportService
{
    Task<Guid> GenerateReportAsync(GenerateReportRequestDto request);
    Task<ReportStatusDto> GetReportStatusAsync(Guid reportId);
    Task<ReportEntity> GetReportByIdAsync(Guid reportId);
    Task<IEnumerable<ReportSummaryDto>> GetAllReportsAsync();
    Task<List<ReportHotelDto>> GetAllHotelsByCity(string city);
    Task SaveReportAsync(ReportEntity reportEntity);
}