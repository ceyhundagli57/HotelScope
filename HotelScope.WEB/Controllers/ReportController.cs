using System.Text;
using System.Text.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HotelScope.WEB.Controllers;

public class ReportController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public ReportController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7006");

    }
    public IActionResult GenerateReport()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateReport(GenerateReportRequestDto request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/reports/request", content);

        if (response.IsSuccessStatusCode)
        {
            var reportInfo = await response.Content.ReadFromJsonAsync<ReportResponseDto>();
            return RedirectToAction("Status", new { reportId = reportInfo.ReportId });
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError("", errorMessage);
        return View(request);
    }
   
    
    public async Task<IActionResult> Status(Guid reportId)
    {
        var response = await _httpClient.GetAsync($"/reports/{reportId}/status");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.ErrorMessage = "Unable to fetch report status.";
            return View("Error");
        }

        var reportStatus = await response.Content.ReadFromJsonAsync<ReportStatusDto>();

        return View(reportStatus);
    }
    
    public async Task<IActionResult> Details(Guid reportId)
    {
        var response = await _httpClient.GetAsync($"/reports/{reportId}");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.ErrorMessage = "Unable to fetch report details.";
            return View("Error");
        }

        var report = await response.Content.ReadFromJsonAsync<ReportDto>();
        return View(report);
    }


    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("/reports");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.ErrorMessage = "Unable to fetch reports.";
            return View("Error");
        }

        var reports = await response.Content.ReadFromJsonAsync<List<ReportSummaryDto>>();
        return View(reports);
    }
    
}