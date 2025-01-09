using System.Text;
using System.Text.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HotelScope.WEB.Controllers;

public class HotelStaffController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly ILogger<HotelStaffController> _logger;


    public HotelStaffController(IHttpClientFactory httpClientFactory, ILogger<HotelStaffController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7006");
        _logger = logger;


    }
    
    public IActionResult AddHotelStaff(Guid hotelId)
    {
        _logger.LogInformation("AddHotelStaff (GET) method accessed for Hotel ID: {HotelId}", hotelId);
        var model = new CreateStaffDto() { HotelId = hotelId };
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> AddHotelStaff(CreateStaffDto staffDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for AddHotelStaff request ");
            return View(staffDto);
        }

        var content = new StringContent(JsonSerializer.Serialize(staffDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/hotel-staff", content);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("AddHotelStaff (POST) method started for Staff ");

            return RedirectToAction("Details", "Hotel", new { hotelId = staffDto.HotelId });
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        _logger.LogWarning("Failed to add hotel staff");

        ModelState.AddModelError("", errorMessage);
        return View(staffDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteHotelStaff(Guid staffId, Guid hotelId)
    {
        var response = await _httpClient.DeleteAsync($"/hotel-staff/{staffId}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Hotel staff deleted successfully for Staff ID: {StaffId}, Hotel ID: {HotelId}", staffId, hotelId);
            return RedirectToAction("Details", "Hotel", new { hotelId });
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        _logger.LogWarning("Failed to delete hotel staff. Staff ID: {StaffId}, Hotel ID: {HotelId}. Error: {ErrorMessage}", staffId, hotelId, errorMessage);

        ViewBag.ErrorMessage = errorMessage;
        return View("Error");
    }
    
}