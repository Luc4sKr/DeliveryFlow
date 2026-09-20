using DeliveryService.Api.Application.Abstractions;
using DeliveryService.Api.Domain;

namespace DeliveryService.Api.Application.Deliveries;

public class CreateDeliveryUseCase(
    IDeliveryRepository repository,
    IEventPublisher eventPublisher,
    string deliveryCreatedExchange)
{
    public async Task<Delivery> ExecuteAsync(
        CreateDeliveryCommand command,
        CancellationToken cancellationToken = default)
    {
        var address = new Address(
            command.Street,
            command.Number,
            command.Complement,
            command.Neighborhood,
            command.City,
            command.State,
            command.ZipCode,
            command.Country);
        var delivery = Delivery.Create(address, command.Weight, command.Volume, DateTime.UtcNow);

        repository.Add(delivery);
        await eventPublisher.PublishAsync(deliveryCreatedExchange, delivery, cancellationToken);

        return delivery;
    }
}