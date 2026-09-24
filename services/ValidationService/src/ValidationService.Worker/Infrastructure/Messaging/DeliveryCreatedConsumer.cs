using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ValidationService.Worker.Application.Validation;
using ValidationService.Worker.Contracts.Events;

namespace ValidationService.Worker.Infrastructure.Messaging;

public class DeliveryCreatedConsumer(
    IOptions<RabbitMqOptions> options,
    DeliveryValidator validator,
    RabbitMqEventPublisher publisher,
    ILogger<DeliveryCreatedConsumer> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (string.IsNullOrWhiteSpace(options.Value.ConnectionString))
        {
            logger.LogWarning("RabbitMQ nao configurado. O consumidor de validacao nao foi iniciado.");
            return Task.CompletedTask;
        }

        var factory = new ConnectionFactory { Uri = new Uri(options.Value.ConnectionString) };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.ExchangeDeclare(options.Value.DeliveryCreatedExchange, ExchangeType.Fanout, durable: true, autoDelete: false);
        channel.QueueDeclare(options.Value.QueueName, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(options.Value.QueueName, options.Value.DeliveryCreatedExchange, string.Empty);
        channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (_, eventArgs) =>
        {
            try
            {
                var delivery = JsonSerializer.Deserialize<DeliveryCreatedEvent>(eventArgs.Body.Span,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (delivery is null)
                    throw new JsonException("Mensagem de entrega vazia.");

                var errors = validator.Validate(delivery);
                var result = new ValidationCompletedEvent(
                    delivery.Id,
                    errors.Count == 0,
                    errors,
                    DateTime.UtcNow);

                publisher.PublishAsync(result, stoppingToken).GetAwaiter().GetResult();
                channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
                logger.LogInformation("Entrega {DeliveryId} validada: {IsValid}.", delivery.Id, result.IsValid);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Falha ao processar mensagem de entrega.");
                channel.BasicNack(eventArgs.DeliveryTag, multiple: false, requeue: false);
            }
        };

        channel.BasicConsume(options.Value.QueueName, autoAck: false, consumer);
        stoppingToken.Register(() =>
        {
            channel.Close();
            connection.Close();
            channel.Dispose();
            connection.Dispose();
        });

        return Task.Delay(Timeout.Infinite, stoppingToken);
    }
}