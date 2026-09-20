namespace DeliveryService.Api.Infrastructure.Messaging;

public class RabbitMqOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DeliveryCreatedExchange { get; set; } = "delivery.created";
}