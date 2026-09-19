using DeliveryService.Api.Application.Abstractions;
using DeliveryService.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Api.Infrastructure.Persistence;

public class DeliveryRepository(DeliveryDbContext dbContext) : IDeliveryRepository
{
    public Delivery Add(Delivery delivery)
    {
        dbContext.Deliveries.Add(delivery);
        dbContext.SaveChanges();
        return delivery;
    }

    public Delivery? GetById(Guid id)
    {
        return dbContext.Deliveries
            .Include(delivery => delivery.Destination)
            .SingleOrDefault(delivery => delivery.Id == id);
    }
}