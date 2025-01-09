using Domain.Entities;

namespace Infrastructure.Persistence;

public class HotelStaffRepository : Repository <HotelStaffEntity>
{
private ApplicationDbContext _db;

public HotelStaffRepository(ApplicationDbContext db) : base(db)
{
    _db = db;
}
}