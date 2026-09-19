using DeliveryService.Api.Domain;

namespace DeliveryService.Api.Contracts.Responses;

public record DeliveryResponse(
    Guid Id,
    AddressResponse Destination,
    decimal Weight,
    decimal Volume,
    DeliveryStatus Status,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static DeliveryResponse FromDomain(Delivery delivery) => new(
        delivery.Id,
        new AddressResponse(
            delivery.Destination.Street,
            delivery.Destination.Number,
            delivery.Destination.Complement,
            delivery.Destination.Neighborhood,
            delivery.Destination.City,
            delivery.Destination.State,
            delivery.Destination.ZipCode,
            delivery.Destination.Country),
        delivery.Weight,
        delivery.Volume,
        delivery.Status,
        delivery.CreatedAt,
        delivery.UpdatedAt);
}

public record AddressResponse(
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string Country);