package main

import (
	"context"
	"github.com/Luc4sKr/DeliveryFlow/services/PriorityService/internal/config"
	"github.com/Luc4sKr/DeliveryFlow/services/PriorityService/internal/container"
	dotenv "github.com/joho/godotenv"
	"log"
)

func main() {
	_ = dotenv.Load()

	cfg, err := config.Load()
	if err != nil {
		log.Fatalf("%s", err.Error())
	}
	ctx := context.Background()
	cont := container.New(cfg)

	if err := cont.Start(ctx); err != nil {
		log.Fatalf("error starting application: %v", err)
	}
}
