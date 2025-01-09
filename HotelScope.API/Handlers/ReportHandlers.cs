using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace HotelScope.API.Handlers;

public class ReportHandlers
{
     public static async Task<IResult> GenerateReport(
        GenerateReportRequestDto request,
        IReportService reportService)
    {
        try
        {
            var reportId = await reportService.GenerateReportAsync(request);
            return Results.Accepted($"/reports/{reportId}", new { ReportId = reportId });
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
        
        
    }

    public static async Task<IResult> CreateReport(SaveReportRequestDto request, IReportService reportService)
    {
        
        var reportDto = await reportService.GetAllHotelsByCity(request.Location);
        var reportFromDb = await reportService.GetReportByIdAsync(request.Id);


        reportFromDb.Location = request.Location;
        //reportFromDb.HotelCount = reportDto.Count;
        reportFromDb.HotelCount = reportDto
            .Where(h => h.Contacts.Any(c => c.Location == request.Location))
            .Select(h => h.Hotel.Id)
            .Distinct()
            .Count();


        reportFromDb.HotelContactPhoneCount = reportDto
            .SelectMany(h => h.Contacts ?? new List<HotelContactInfoEntity>()) // Flatten contacts
            .Where(c => c.Location == request.Location && !string.IsNullOrEmpty(c.PhoneNumber)) // Filter by location and exclude null/empty phone numbers
            .Select(c => c.PhoneNumber) // Extract phone numbers
            .Distinct() // Get unique phone numbers
            .Count();    
        reportFromDb.Status = ReportStatus.Completed;
                
        await reportService.SaveReportAsync(reportFromDb);
        return Results.Ok();
    }

    public static async Task<IResult> GetReportStatus(
        Guid reportId,
        IReportService reportService)
    {
        try
        {
            var status = await reportService.GetReportStatusAsync(reportId);
            return Results.Ok(status);
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> GetReportById(
        Guid reportId,
        IReportService reportService)
    {
        try
        {
            var report = await reportService.GetReportByIdAsync(reportId);
            return Results.Ok(report);
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> GetAllReports(
        IReportService reportService)
    {
        try
        {
            var reports = await reportService.GetAllReportsAsync();
            return Results.Ok(reports);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}