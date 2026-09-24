using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using ValidationService.Worker.Contracts.Events;

namespace ValidationService.Worker.Infrastructure.Messaging;

public class RabbitMqEventPublisher(
    IOptions<RabbitMqOptions> options,
    ILogger<RabbitMqEventPublisher> logger)
{
    public Task PublishAsync(ValidationCompletedEvent message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var factory = new ConnectionFactory { Uri = new Uri(options.Value.ConnectionString) };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(options.Value.ValidationCompletedExchange, ExchangeType.Fanout, durable: true, autoDelete: false);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(options.Value.ValidationCompletedExchange, string.Empty, properties, body);
        }
        catch (Exception exception) when (exception is BrokerUnreachableException or ConnectFailureException)
        {
            logger.LogWarning(exception, "RabbitMQ indisponivel. O evento nao foi publicado.");
        }

        return Task.CompletedTask;
    }
}