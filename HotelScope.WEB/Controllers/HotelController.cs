using System.Text;
using System.Text.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HotelScope.WEB.Controllers;

public class HotelController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    
    public HotelController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7006");

    }
    // GET
    public async Task<IActionResult> Index()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("hotels");

            response.EnsureSuccessStatusCode();

            
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<HotelDto>>(jsonResponse);

            // Output the response
            Console.WriteLine("Response received:");
            Console.WriteLine(result);
            return View(result.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return View();
    }
    [HttpGet]
    public IActionResult AddHotel()
    {
        return View(); 
    }
    [HttpPost]
    public async Task<IActionResult> AddHotel(CreateHotelDto hotelDto)
    {
        try
        {
            string jsonContent = JsonSerializer.Serialize(hotelDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("hotels", content);

            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var createdHotel = JsonSerializer.Deserialize<CreateHotelDto>(jsonResponse);

            return RedirectToAction("Index"); 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ViewBag.ErrorMessage = "An error occurred while adding the hotel.";
            return View(hotelDto); 
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteHotel(Guid hotelId)
    {
        var response = await _httpClient.DeleteAsync($"/hotels/{hotelId}");

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index"); 
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        ViewBag.ErrorMessage = errorMessage;
        return View("Error");
    }
 

    public async Task<IActionResult> Details(Guid hotelId)
    {
        var response = await _httpClient.GetAsync($"/hotels/{hotelId}");

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            ViewBag.ErrorMessage = errorMessage;
            return View("Error");
        }

        var hotelJson = await response.Content.ReadAsStringAsync();
        var hotel = JsonSerializer.Deserialize<HotelDto>(hotelJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return View(hotel);
    }
    
}