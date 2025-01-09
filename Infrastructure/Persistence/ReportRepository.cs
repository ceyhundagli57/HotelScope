using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ReportRepository : Repository<ReportEntity>
{
    private readonly ApplicationDbContext _context;

    public ReportRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

}