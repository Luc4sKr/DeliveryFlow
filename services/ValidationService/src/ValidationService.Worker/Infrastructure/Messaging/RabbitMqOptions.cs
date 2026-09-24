namespace ValidationService.Worker.Infrastructure.Messaging;

public class RabbitMqOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DeliveryCreatedExchange { get; set; } = "delivery.created";
    public string ValidationCompletedExchange { get; set; } = "validation.completed";
    public string QueueName { get; set; } = "validation-service";
}