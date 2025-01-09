using Application.DTOs;
using Application.Interfaces;

namespace HotelScope.API.Handlers;

public class HotelStaffHandlers
{
     public static async Task<IResult> AddHotelStaff(
        CreateStaffDto staffDto,
        IHotelStaffService staffService)
    {
        try
        {
            await staffService.AddStaffAsync(staffDto);
            return Results.Created($"/hotel-staff/{staffDto.HotelId}", staffDto);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
     
    public static async Task<IResult> UpdateHotelStaff(
        UpdateStaffDto staffDto,
        IHotelStaffService staffService)
    {
        try
        {
            await staffService.UpdateStaffAsync(staffDto);
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

    public static async Task<IResult> DeleteHotelStaff(
        Guid staffId,
        IHotelStaffService staffService)
    {
        try
        {
            await staffService.DeleteStaffAsync(staffId);
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


    public static async Task<IResult> GetStaffByHotelId(
        Guid hotelId,
        IHotelStaffService staffService)
    {
        try
        {
            var staffMembers = await staffService.GetStaffByHotelIdAsync(hotelId);
            return Results.Ok(staffMembers);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}