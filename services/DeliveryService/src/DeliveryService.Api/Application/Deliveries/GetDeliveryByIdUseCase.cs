using DeliveryService.Api.Application.Abstractions;
using DeliveryService.Api.Domain;

namespace DeliveryService.Api.Application.Deliveries;

public class GetDeliveryByIdUseCase(IDeliveryRepository repository)
{
    public Delivery? Execute(Guid id) => repository.GetById(id);
}