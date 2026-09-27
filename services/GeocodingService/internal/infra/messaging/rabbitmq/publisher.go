package rabbitmq

import (
	"context"
	"errors"
	ports "github.com/Luc4sKr/DeliveryFlow/services/GeocodingService/internal/application/ports"
	rmq "github.com/rabbitmq/rabbitmq-amqp-go-client/pkg/rabbitmqamqp"
)

type Publisher struct {
	publisher *rmq.Publisher
}

func NewPublisher(publisher *rmq.Publisher) *Publisher {
	return &Publisher{
		publisher: publisher,
	}
}

func (p *Publisher) Publish(ctx context.Context, message []byte) error {
	res, err := p.publisher.Publish(ctx, rmq.NewMessage([]byte(message)))
	if err != nil {
		return err
	}
	switch res.Outcome.(type) {
	case *rmq.StateAccepted:
		return nil
	default:
		return errors.New("failed to publish message")
	}
}

var _ ports.EventPublisher = (*Publisher)(nil)
