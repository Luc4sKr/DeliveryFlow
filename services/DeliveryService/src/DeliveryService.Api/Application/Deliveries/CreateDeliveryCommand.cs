namespace DeliveryService.Api.Application.Deliveries;

public record CreateDeliveryCommand(
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string Country,
    decimal Weight,
    decimal Volume);