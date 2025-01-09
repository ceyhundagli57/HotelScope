using System.Text;
using System.Text.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HotelScope.WEB.Controllers;

public class HotelStaffController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public HotelStaffController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7006");

    }
    
    public IActionResult AddHotelStaff(Guid hotelId)
    {
        var model = new CreateStaffDto() { HotelId = hotelId };
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> AddHotelStaff(CreateStaffDto staffDto)
    {
        if (!ModelState.IsValid)
            return View(staffDto);

        var content = new StringContent(JsonSerializer.Serialize(staffDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/hotel-staff", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Details", "Hotel", new { hotelId = staffDto.HotelId });
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError("", errorMessage);
        return View(staffDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteHotelStaff(Guid staffId, Guid hotelId)
    {
        var response = await _httpClient.DeleteAsync($"/hotel-staff/{staffId}");

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Details", "Hotel", new { hotelId });
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        ViewBag.ErrorMessage = errorMessage;
        return View("Error");
    }
    
}