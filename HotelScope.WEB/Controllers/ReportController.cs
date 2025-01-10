using System.Text;
using System.Text.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HotelScope.WEB.Controllers;

public class ReportController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReportController> _logger;


    public ReportController(IHttpClientFactory httpClientFactory,  ILogger<ReportController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiUrl") ?? throw new NotImplementedException());
        _logger = logger;

    }
    public IActionResult GenerateReport()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateReport(GenerateReportRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for GenerateReport request.");
            return View(request);
        }

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("reports/request", content);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("GenerateReport (POST) method started for Request: {@Request}", request);
            var reportInfo = await response.Content.ReadFromJsonAsync<ReportResponseDto>();
            return RedirectToAction("Status", new { reportId = reportInfo.ReportId });
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        _logger.LogWarning("Report generation request failed. Error: {ErrorMessage}", errorMessage);

        ModelState.AddModelError("", errorMessage);
        return View(request);
    }
   
    
    public async Task<IActionResult> Status(Guid reportId)
    {
        var response = await _httpClient.GetAsync($"reports/{reportId}/status");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to fetch status for Report ID: {ReportId}.", reportId);
            ViewBag.ErrorMessage = "Unable to fetch report status.";
            return View("Error");
        }

        var reportStatus = await response.Content.ReadFromJsonAsync<ReportStatusDto>();
        _logger.LogInformation("Successfully fetched status for Report ID: {ReportId}", reportId);


        return View(reportStatus);
    }
    
    public async Task<IActionResult> Details(Guid reportId)
    {
        var response = await _httpClient.GetAsync($"reports/{reportId}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to fetch details for Report ID: {ReportId}.", reportId);
            ViewBag.ErrorMessage = "Unable to fetch report details.";
            return View("Error");
        }
        _logger.LogInformation("Successfully fetched details for Report ID: {ReportId}", reportId);
        var report = await response.Content.ReadFromJsonAsync<ReportDto>();
        return View(report);
    }


    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("reports");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to fetch reports.");
            ViewBag.ErrorMessage = "Unable to fetch reports.";
            return View("Error");
        }
        
        _logger.LogInformation("Successfully fetched reports list.");
        var reports = await response.Content.ReadFromJsonAsync<List<ReportSummaryDto>>();
        return View(reports?.OrderByDescending(x => x.RequestDate).ToList());
    }
    
}