using Application.DTOs;

namespace Application.Interfaces;

public interface IHotelContactInfoService
{
    Task<IEnumerable<HotelContactInfoDto>> GetContactInfosByHotelIdAsync(Guid hotelId);
    Task AddContactInfoAsync(CreateContactInfoDto contactInfoDto);
    Task UpdateContactInfoAsync(UpdateContactInfoDto contactInfoDto);
    Task DeleteContactInfoAsync(Guid contactInfoId);
}