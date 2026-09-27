package config

import (
	"errors"
	"os"
)

type Config struct {
	RabbitMqConnectionString string
	QueueName                string
	DeliveryCreationExchange string
	DeliveryRegionalizationQueue string
}

func Load() (*Config, error) {
	connectionString, exists := os.LookupEnv("RABBIT_MQ_URL")
	if !exists {
		return nil, errors.New("env not found: RABBIT_MQ_URL")
	}

	queueName := getEnvOrDefault("GEOCODING_QUEUE", "geocoding")
	deliveryCreationExchange := getEnvOrDefault("DELIVERY_CREATION_EXCHANGE", "delivery.created")
	deliveryRegionalizationQueue := getEnvOrDefault("DELIVERY_REGIONALIZATION_QUEUE", "regionalization")

	return &Config{
		RabbitMqConnectionString: connectionString,
		QueueName:                queueName,
		DeliveryCreationExchange: deliveryCreationExchange,
		DeliveryRegionalizationQueue: deliveryRegionalizationQueue,
	}, nil
}

func getEnvOrDefault(key string, defaultValue string) string {
	value, exists := os.LookupEnv(key)
	if exists {
		return value
	}
	return defaultValue
}
