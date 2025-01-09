using System.Text;
using System.Text.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HotelScope.WEB.Controllers;

public class HotelContactInfoController:Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public HotelContactInfoController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7006");

    }
    public IActionResult AddContactInfo(Guid hotelId)
    {
        var model = new CreateContactInfoDto { HotelId = hotelId };
        return View(model);
    }

   
    [HttpPost]
    public async Task<IActionResult> AddContactInfo(CreateContactInfoDto contactInfoDto)
    {
        if (!ModelState.IsValid)
            return View(contactInfoDto);

        var content = new StringContent(JsonSerializer.Serialize(contactInfoDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/contact-info", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Details", "Hotel", new { hotelId = contactInfoDto.HotelId });
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError("", errorMessage);
        return View(contactInfoDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteContactInfo(Guid contactInfoId, Guid hotelId)
    {
        var response = await _httpClient.DeleteAsync($"/contact-info/{contactInfoId}");

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Details", "Hotel", new { hotelId });
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        ViewBag.ErrorMessage = errorMessage;
        return View("Error");
    }
    
}