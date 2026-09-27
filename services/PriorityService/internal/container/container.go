package container

import (
	"context"
	"github.com/Luc4sKr/DeliveryFlow/services/PriorityService/internal/application/handler"
	"github.com/Luc4sKr/DeliveryFlow/services/PriorityService/internal/config"
	"github.com/Luc4sKr/DeliveryFlow/services/PriorityService/internal/infra/messaging/rabbitmq"
	rmq "github.com/rabbitmq/rabbitmq-amqp-go-client/pkg/rabbitmqamqp"
)

type Container struct {
	cfg *config.Config
}

func New(cfg *config.Config) *Container {
	return &Container{cfg: cfg}
}

func (c *Container) Start(ctx context.Context) error {
	rabbitMqEnvironment := rmq.NewEnvironment(c.cfg.RabbitMqConnectionString, nil)
	connection, err := rabbitMqEnvironment.NewConnection(ctx)

	if err != nil {
		return err
	}

	defer rabbitMqEnvironment.CloseConnections(ctx)

	management := connection.Management()

	_, err = management.DeclareExchange(ctx, &rmq.FanOutExchangeSpecification{
		Name: c.cfg.DeliveryCreationExchange,
	})
	if err != nil {
		return err
	}

	_, err = management.DeclareQueue(ctx, &rmq.DefaultQueueSpecification{Name: c.cfg.QueueName})
	if err != nil {
		return err
	}

	_, err = management.Bind(ctx, &rmq.ExchangeToQueueBindingSpecification{
		SourceExchange:   c.cfg.DeliveryCreationExchange,
		DestinationQueue: c.cfg.QueueName,
	})
	if err != nil {
		return err
	}

	_, err = management.DeclareQueue(ctx, &rmq.DefaultQueueSpecification{Name: c.cfg.DeliveryPreparationQueue})
	if err != nil {
		return err
	}

	rabbitMqConsumer, err := connection.NewConsumer(ctx, c.cfg.QueueName, nil)
	if err != nil {
		return err
	}

	rabbitmqPublisher, err := connection.NewPublisher(ctx, &rmq.QueueAddress{Queue: c.cfg.DeliveryPreparationQueue}, nil)

	consumer := rabbitmq.NewConsumer(rabbitMqConsumer)
	publisher := rabbitmq.NewPublisher(rabbitmqPublisher)

	priorityHandler := handler.NewPriorityHandler(publisher)

	return consumer.Consume(ctx, priorityHandler.Handle)
}
