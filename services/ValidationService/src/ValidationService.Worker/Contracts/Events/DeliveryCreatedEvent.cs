namespace ValidationService.Worker.Contracts.Events;

public class DeliveryCreatedEvent
{
    public Guid Id { get; set; }
    public AddressEvent Destination { get; set; } = new();
    public decimal Weight { get; set; }
    public decimal Volume { get; set; }
}

public class AddressEvent
{
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}