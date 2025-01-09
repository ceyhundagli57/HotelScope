using Application.DTOs;
using Application.Interfaces;

namespace HotelScope.API.Handlers;

public class HotelContactInfoHandlers
{
    public static async Task<IResult> AddContactInfo(
        CreateContactInfoDto contactInfoDto,
        IHotelContactInfoService contactInfoService)
    {
        try
        {
            await contactInfoService.AddContactInfoAsync(contactInfoDto);
            return Results.Created($"/contact-info/{contactInfoDto.HotelId}", contactInfoDto);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
    
    public static async Task<IResult> UpdateContactInfo(
        UpdateContactInfoDto contactInfoDto,
        IHotelContactInfoService contactInfoService)
    {
        try
        {
            await contactInfoService.UpdateContactInfoAsync(contactInfoDto);
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

    public static async Task<IResult> DeleteContactInfo(
        Guid contactInfoId,
        IHotelContactInfoService contactInfoService)
    {
        try
        {
            await contactInfoService.DeleteContactInfoAsync(contactInfoId);
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
    
    public static async Task<IResult> GetContactInfosByHotelId(
        Guid hotelId,
        IHotelContactInfoService contactInfoService)
    {
        try
        {
            var contactInfos = await contactInfoService.GetContactInfosByHotelIdAsync(hotelId);
            return Results.Ok(contactInfos);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}