using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<HotelEntity> Hotels { get; set; }
    public DbSet<HotelStaffEntity> HotelStaffs { get; set; }
    public DbSet<HotelContactInfoEntity> HotelContactInfos { get; set; }
    public DbSet<ReportEntity> Reports { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base (options)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql( 
            "Server=database;Database=HotelScope;User Id=root;Password=ceyhun1010;Port=3306;SslMode=Preferred;",  
            new MySqlServerVersion(new Version(8,0,38))
        );
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HotelEntity>().HasKey(p => p.Id);
        base.OnModelCreating(modelBuilder);
    }
}