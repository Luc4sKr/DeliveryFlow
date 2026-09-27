package rabbitmq

import (
	"context"
	"errors"
	rmq "github.com/rabbitmq/rabbitmq-amqp-go-client/pkg/rabbitmqamqp"
	"log"
)

type Handler func(context.Context, []byte) error

type Consumer struct {
	rabbitMqConsumer *rmq.Consumer
}

func NewConsumer(consumer *rmq.Consumer) *Consumer {
	return &Consumer{rabbitMqConsumer: consumer}
}

func (c *Consumer) Consume(ctx context.Context, handler Handler) error {
	for {
		delivery, err := c.rabbitMqConsumer.Receive(ctx)
		if err != nil && errors.Is(err, context.Canceled) {
			return err
		}

		if err != nil {
			log.Printf("failed to receive message: %v", err)
			_ = delivery.Accept(ctx)
			continue
		}

		msg := delivery.Message()
		go func() {
			err := handler(ctx, msg.GetData())
			if err != nil {
				log.Printf("failed to execute handler: %v", err)
			}
			_ = delivery.Accept(ctx)
		}()

	}

}
