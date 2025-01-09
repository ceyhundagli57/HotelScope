using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence;

public class UnitOfWork: IUnitOfWork
{
    private ApplicationDbContext _db;

    public IRepository<HotelEntity> Hotel { get; }
    public IRepository<HotelContactInfoEntity> HotelContactInfo { get; }
    public IRepository<HotelStaffEntity> HotelStaff { get; }
    public IRepository<ReportEntity> Report { get; }
    
    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Hotel = new HotelRepository(_db);
        HotelContactInfo = new HotelContactInfoRepository(_db);
        HotelStaff = new HotelStaffRepository(_db);
        // Report = new ReportRepository(_db);
        Report = new ReportRepository(_db);
    }
    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
    
    public async Task<List<ReportHotelDto>> GetAllHotelsByCity(string city)
    {
        var contacts = await HotelContactInfo.GetAllAsync();
        var hotels = await Hotel.GetAllAsync();
        var query = from hotel in hotels
            join contact in contacts
                on hotel.Id equals contact.HotelId
            where contact.Location.Equals(city)
            select new ReportHotelDto()
            {
                Hotel = hotel,
                Contacts = contacts.ToList(),
            };
        return query.ToList();
    }
    
}