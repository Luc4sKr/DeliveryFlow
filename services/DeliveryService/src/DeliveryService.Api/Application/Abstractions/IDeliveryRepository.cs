using DeliveryService.Api.Domain;

namespace DeliveryService.Api.Application.Abstractions;

public interface IDeliveryRepository
{
    Delivery Add(Delivery delivery);
    Delivery? GetById(Guid id);
}