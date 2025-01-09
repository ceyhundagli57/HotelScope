using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUnitOfWork
{
    IRepository<HotelEntity> Hotel { get; }
    IRepository<HotelContactInfoEntity> HotelContactInfo { get; }
    IRepository<HotelStaffEntity> HotelStaff { get; }
    
    IRepository<ReportEntity> Report { get; }
    
    Task SaveAsync();
    Task<List<ReportHotelDto>> GetAllHotelsByCity(string city);

}