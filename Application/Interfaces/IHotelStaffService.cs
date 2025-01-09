using Application.DTOs;

namespace Application.Interfaces;

public interface IHotelStaffService
{
    Task<IEnumerable<HotelStaffDto>> GetStaffByHotelIdAsync(Guid hotelId);
    Task AddStaffAsync(CreateStaffDto staffDto);
    Task UpdateStaffAsync(UpdateStaffDto staffDto);
    Task DeleteStaffAsync(Guid staffId);
}