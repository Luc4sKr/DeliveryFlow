namespace ValidationService.Worker.Infrastructure.Messaging;

public class RabbitMqOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DeliveryCreatedExchange { get; set; } = "delivery.created";
    public string RegionalizationQueue { get; set; } = "regionalization";
    public string QueueName { get; set; } = "validation-service";
}