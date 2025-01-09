using System.Text;
using System.Text.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HotelScope.WEB.Controllers;

public class HotelController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly ILogger<HotelController> _logger;

    public HotelController(IHttpClientFactory httpClientFactory, ILogger<HotelController> logger )
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiUrl") ?? throw new NotImplementedException());
        _logger = logger;

    }
    // GET
    public async Task<IActionResult> Index()
    {
        try
        {
            _logger.LogInformation("Index method started.");
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
            _logger.LogError("An error occurred while fetching hotel data."); // Log error
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
            _logger.LogInformation("AddHotel method started with hotel name: {HotelName}", hotelDto.CompanyTitle);

            string jsonContent = JsonSerializer.Serialize(hotelDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("hotels", content);

            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var createdHotel = JsonSerializer.Deserialize<CreateHotelDto>(jsonResponse);
            
            _logger.LogInformation("Hotel successfully added:");


            return RedirectToAction("Index"); 
        }
        catch (Exception e)
        {
            _logger.LogError( "An error occurred while adding the hotel with name: {HotelName}", hotelDto.CompanyTitle);

            Console.WriteLine(e);
            ViewBag.ErrorMessage = "An error occurred while adding the hotel.";
            return View(hotelDto); 
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteHotel(Guid hotelId)
    {
        _logger.LogInformation("DeleteHotel method started for Hotel ID: {HotelId}", hotelId);

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
        _logger.LogInformation("Details method started for Hotel ID: {HotelId}", hotelId);

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