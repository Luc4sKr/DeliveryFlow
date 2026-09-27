package container

import (
	"context"
	"github.com/Luc4sKr/DeliveryFlow/services/GeocodingService/internal/application/handler"
	"github.com/Luc4sKr/DeliveryFlow/services/GeocodingService/internal/config"
	"github.com/Luc4sKr/DeliveryFlow/services/GeocodingService/internal/infra/messaging/rabbitmq"
	"github.com/Luc4sKr/DeliveryFlow/services/GeocodingService/internal/infra/openstreetmaps"
	rmq "github.com/rabbitmq/rabbitmq-amqp-go-client/pkg/rabbitmqamqp"
	"net/http"
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

	_, err = management.DeclareQueue(ctx, &rmq.DefaultQueueSpecification{Name: c.cfg.DeliveryRegionalizationQueue})
	if err != nil {
		return err
	}

	rabbitMqConsumer, err := connection.NewConsumer(ctx, c.cfg.QueueName, nil)
	if err != nil {
		return err
	}

	rabbitmqPublisher, err := connection.NewPublisher(ctx, &rmq.QueueAddress{Queue: c.cfg.DeliveryRegionalizationQueue}, nil)

	consumer := rabbitmq.NewConsumer(rabbitMqConsumer)
	publisher := rabbitmq.NewPublisher(rabbitmqPublisher)

	geocoder := geocoding.NewGeoCoder(&http.Client{})
	geocodingHandler := handler.NewGeoCodingHandler(publisher, geocoder)

	return consumer.Consume(ctx, geocodingHandler.Handle)
}
