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
    public Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            var settings = options.Value;
            var factory = new ConnectionFactory
            {
                HostName = settings.HostName,
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(topic, durable: true, exclusive: false, autoDelete: false);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(string.Empty, topic, properties, body);
        }
        catch (Exception exception) when (exception is BrokerUnreachableException or ConnectFailureException)
        {
            logger.LogWarning(exception, "RabbitMQ indisponivel. O evento nao foi publicado em {Topic}.", topic);
        }
        return Task.CompletedTask;
    }
}