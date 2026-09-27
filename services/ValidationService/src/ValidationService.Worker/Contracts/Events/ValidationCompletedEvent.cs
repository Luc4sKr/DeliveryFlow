namespace ValidationService.Worker.Contracts.Events;

public record ValidationCompletedEvent(
    Guid Id,
    string Status,
    string Service,
    AddressEvent Destination,
    decimal Weight,
    decimal Volume);