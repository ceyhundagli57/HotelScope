using Application.DTOs;
using Application.Interfaces;

namespace HotelScope.API.Handlers;

public class HotelHandlers
{
      public static async Task<IResult> AddHotel(
        CreateHotelDto hotelDto,
        IHotelService hotelService)
    {
        try
        {
            await hotelService.AddHotelAsync(hotelDto);
            return Results.Created($"/hotels/{hotelDto.CompanyTitle}", hotelDto);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
      
      
    public static async Task<IResult> UpdateHotel(
        UpdateHotelDto hotelDto,
        IHotelService hotelService)
    {
        try
        {
            await hotelService.UpdateHotelAsync(hotelDto);
            return Results.Ok();
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
    
    public static async Task<IResult> DeleteHotel(
        Guid hotelId,
        IHotelService hotelService)
    {
        try
        {
            await hotelService.DeleteHotelAsync(hotelId);
            return Results.NoContent();
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

    public static async Task<IResult> GetHotelById(
        Guid hotelId,
        IHotelService hotelService)
    {
        try
        {
            var hotel = await hotelService.GetHotelByIdAsync(hotelId);
            return Results.Ok(hotel);
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

    public static async Task<IResult> GetAllHotels(
        IHotelService hotelService)
    {
        try
        {
            var hotels = await hotelService.GetAllHotelsAsync();
            return Results.Ok(hotels);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
    
}