using Domain.Entities;

namespace Infrastructure.Persistence;

public class HotelRepository: Repository<HotelEntity> 
{
    private ApplicationDbContext _db;

    public HotelRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    
}