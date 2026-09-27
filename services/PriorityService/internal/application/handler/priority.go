package handler

import (
	"context"
	"encoding/json"
	"math/rand"
	"github.com/Luc4sKr/DeliveryFlow/services/PriorityService/internal/application/dto/output"
	"github.com/Luc4sKr/DeliveryFlow/services/PriorityService/internal/application/dto/input"
	"github.com/Luc4sKr/DeliveryFlow/services/PriorityService/internal/application/ports"
)

const service = "PriorityService"

const (
	minPriority = 1
	maxPriority = 5
)

type PriorityHandler struct {
	publisher ports.EventPublisher
}

func NewPriorityHandler(publisher ports.EventPublisher) *PriorityHandler {
	return &PriorityHandler{
		publisher: publisher,
	}
}

func (h *PriorityHandler) Handle(ctx context.Context, message []byte) error {

	var delivery input.DeliveryDTO

	if err := json.Unmarshal(message, &delivery); err != nil {
		return err
	}

	priority := h.generatePriority()
	payload, _ := json.Marshal(output.DeliveryDTO{
		Id: delivery.Id,
		Service: service,
		Status: output.StatusSuccess,
		Priority: priority,
	})
	return h.publisher.Publish(ctx, payload)
}

func (h *PriorityHandler) generatePriority() int {
	priority := int(rand.Float64() * maxPriority) + minPriority
	return priority
}
