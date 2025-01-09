using Application.DTOs;

namespace Application.Interfaces;

public interface IHotelService
{
    Task AddHotelAsync(CreateHotelDto hotelDto);
    Task UpdateHotelAsync(UpdateHotelDto hotelDto);

    Task DeleteHotelAsync(Guid hotelId);

    Task<HotelDto> GetHotelByIdAsync(Guid hotelId);

    Task<IEnumerable<HotelDto>> GetAllHotelsAsync();
}