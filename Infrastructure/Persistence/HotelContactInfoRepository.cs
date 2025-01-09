using Domain.Entities;

namespace Infrastructure.Persistence;

public class HotelContactInfoRepository : Repository<HotelContactInfoEntity>
{
    private ApplicationDbContext _db;
    public HotelContactInfoRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    
}