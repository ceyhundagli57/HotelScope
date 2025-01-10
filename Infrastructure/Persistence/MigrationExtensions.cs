using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class MigrationExtensions
{
    public static void ApplyMigrationsAndSeed(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Apply pending migrations
        context.Database.Migrate();

        var hotelid1 = Guid.NewGuid();
        var hotelid2 = Guid.NewGuid();
        var hotelid3 = Guid.NewGuid();
        
        // Seed data
        if (!context.Hotels.Any())
        {
            context.Hotels.AddRange(
                new HotelEntity { Id = hotelid1,CompanyTitle = "Marriot"},
                new HotelEntity { Id = hotelid2,CompanyTitle = "Kaya" },
                new HotelEntity { Id = hotelid3,CompanyTitle = "Hilton" }
            );

            context.SaveChanges();
        }
        
        if (!context.HotelContactInfos.Any())
        {
            context.HotelContactInfos.AddRange(
                new HotelContactInfoEntity { Id = Guid.NewGuid(),HotelId = hotelid1,Location = "istanbul",PhoneNumber = "425424235235",Email = "marriot@ifo"},
                new HotelContactInfoEntity { Id = Guid.NewGuid(),HotelId = hotelid2,Location = "istanbul",PhoneNumber = "535424235235",Email = "kayaa@ifo"},
                new HotelContactInfoEntity { Id = Guid.NewGuid(),HotelId = hotelid3,Location = "ankara",PhoneNumber = "1154244245235",Email = "hilton@ifo"}
            );

            context.SaveChanges();
        }
        
        
        if (!context.HotelStaffs.Any())
        {
            context.HotelStaffs.AddRange(
                new HotelStaffEntity { Id = Guid.NewGuid(),HotelId = hotelid1, FirstName = "James", LastName = "Michael"},
                new HotelStaffEntity { Id = Guid.NewGuid(),HotelId = hotelid2, FirstName = "Ali", LastName = "Muhammed"},
                new HotelStaffEntity { Id = Guid.NewGuid(),HotelId = hotelid3, FirstName = "Ahmet", LastName = "Kamil"}
            );

            context.SaveChanges();
        }
    }
}