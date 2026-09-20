using System.Text;
using System.Text.Json;
using DeliveryService.Api.Application.Abstractions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace DeliveryService.Api.Infrastructure.Messaging;

public class RabbitMqEventPublisher(
    IOptions<RabbitMqOptions> options,
    ILogger<RabbitMqEventPublisher> logger) : IEventPublisher
{
    public Task PublishAsync<T>(string exchange, T message, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(options.Value.ConnectionString)
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: true, autoDelete: false);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchange, string.Empty, properties, body);
        }
        catch (Exception exception) when (exception is BrokerUnreachableException or ConnectFailureException)
        {
            logger.LogWarning(exception, "RabbitMQ indisponivel. O evento nao foi publicado em {Exchange}.", exchange);
        }
        return Task.CompletedTask;
    }
}