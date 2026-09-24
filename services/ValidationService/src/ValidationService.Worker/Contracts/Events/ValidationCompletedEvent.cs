namespace ValidationService.Worker.Contracts.Events;

public record ValidationCompletedEvent(
    Guid DeliveryId,
    bool IsValid,
    IReadOnlyCollection<string> Errors,
    DateTime ValidatedAt);