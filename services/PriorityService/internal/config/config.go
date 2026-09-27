package config

import (
	"errors"
	"os"
)

type Config struct {
	RabbitMqConnectionString string
	QueueName                string
	DeliveryCreationExchange string
	DeliveryPreparationQueue string
}

func Load() (*Config, error) {
	var err error

	connectionString, exists := os.LookupEnv("RABBIT_MQ_URL")
	if !exists {
		err = errors.Join(err, errors.New("env not found: RABBIT_MQ_URL"))
	}
	queueName, exists := os.LookupEnv("DELIVERY_QUEUE")
	if !exists {
		err = errors.Join(err, errors.New("env not found: DELIVERY_QUEUE"))
	}
	deliveryCreationExchange, exists := os.LookupEnv("DELIVERY_CREATION_EXCHANGE")
	if !exists {
		err = errors.Join(err, errors.New("env not found: DELIVERY_CREATION_EXCHANGE"))
	}

	deliveryPreparationQueue, exists := os.LookupEnv("DELIVERY_PREPARATION_QUEUE")
	if !exists {
		err = errors.Join(err, errors.New("env not found: DELIVERY_PREPARATION_QUEUE"))
	}

	if err != nil {
		return nil, err
	}

	return &Config{
		RabbitMqConnectionString: connectionString,
		QueueName:                queueName,
		DeliveryCreationExchange: deliveryCreationExchange,
		DeliveryPreparationQueue: deliveryPreparationQueue,
	}, nil
}
