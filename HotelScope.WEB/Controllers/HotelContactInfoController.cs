using System.Text;
using System.Text.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HotelScope.WEB.Controllers;

public class HotelContactInfoController:Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly ILogger<HotelContactInfoController> _logger;


    public HotelContactInfoController(IHttpClientFactory httpClientFactory,ILogger<HotelContactInfoController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7006");

    }
    public IActionResult AddContactInfo(Guid hotelId)
    {
        _logger.LogInformation("AddContactInfo (GET) method accessed for Hotel ID: {HotelId}", hotelId);

        var model = new CreateContactInfoDto { HotelId = hotelId };
        return View(model);
    }

   
    [HttpPost]
    public async Task<IActionResult> AddContactInfo(CreateContactInfoDto contactInfoDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for AddContactInfo request: {@ContactInfoDto}", contactInfoDto);
            return View(contactInfoDto);

        }

        var content = new StringContent(JsonSerializer.Serialize(contactInfoDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/contact-info", content);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Contact info added successfully for Hotel ID: {HotelId}", contactInfoDto.HotelId);
            return RedirectToAction("Details", "Hotel", new { hotelId = contactInfoDto.HotelId });
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        _logger.LogWarning("Failed to add contact info. Error: {ErrorMessage}", errorMessage);

        ModelState.AddModelError("", errorMessage);
        return View(contactInfoDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteContactInfo(Guid contactInfoId, Guid hotelId)
    {
        var response = await _httpClient.DeleteAsync($"/contact-info/{contactInfoId}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("DeleteContactInfo method started for ContactInfo ID: {ContactInfoId}, Hotel ID: {HotelId}", contactInfoId, hotelId);
            return RedirectToAction("Details", "Hotel", new { hotelId });
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        _logger.LogWarning("Failed to delete contact info. ContactInfo ID: {ContactInfoId}, Hotel ID: {HotelId}. Error: {ErrorMessage}", contactInfoId, hotelId, errorMessage);

        ViewBag.ErrorMessage = errorMessage;
        return View("Error");
    }
    
}