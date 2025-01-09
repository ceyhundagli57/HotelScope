using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options : base (options))
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql( 
            "Server=localhost;Database=HotelScope;User Id=root;Password=ceyhun1010;SslMode=Preferred;",  
            new MySqlServerVersion(new Version(8,0,38))
        );
    }
}