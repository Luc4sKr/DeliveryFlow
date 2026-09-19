using DeliveryService.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Api.Infrastructure.Persistence;

public class DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : DbContext(options)
{
    public DbSet<Delivery> Deliveries => Set<Delivery>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var delivery = modelBuilder.Entity<Delivery>();
        delivery.HasKey(item => item.Id);
        delivery.Property(item => item.Status).HasConversion<string>();

        var address = delivery.OwnsOne(item => item.Destination);
        address.Property(item => item.Id);
        address.Property(item => item.Street).IsRequired();
        address.Property(item => item.Number).IsRequired();
        address.Property(item => item.Complement).IsRequired();
        address.Property(item => item.Neighborhood).IsRequired();
        address.Property(item => item.City).IsRequired();
        address.Property(item => item.State).IsRequired();
        address.Property(item => item.ZipCode).IsRequired();
        address.Property(item => item.Country).IsRequired();
    }
}