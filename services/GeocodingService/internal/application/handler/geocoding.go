package handler

import (
	"context"
	"encoding/json"
	"github.com/Luc4sKr/DeliveryFlow/services/GeocodingService/internal/application/dto/input"
	"github.com/Luc4sKr/DeliveryFlow/services/GeocodingService/internal/application/dto/output"
	"github.com/Luc4sKr/DeliveryFlow/services/GeocodingService/internal/application/ports"
	"github.com/Luc4sKr/DeliveryFlow/services/GeocodingService/internal/domain"
)

const service = "GeocodingService"

type GeoCodingHandler struct {
	publisher ports.EventPublisher
	geocoder  ports.GeoCoder
}

func NewGeoCodingHandler(
	publisher ports.EventPublisher,
	geocoder ports.GeoCoder,
) *GeoCodingHandler {
	return &GeoCodingHandler{
		publisher: publisher,
		geocoder:  geocoder,
	}
}

func (h *GeoCodingHandler) Handle(ctx context.Context, message []byte) error {

	var deliveryCreation input.DeliveryCreationDTO
	if err := json.Unmarshal(message, &deliveryCreation); err != nil {
		return err
	}

	geolocation, err := h.geocoder.EncodeAddress(&domain.Address{
		State:  deliveryCreation.Destination.State,
		City:   deliveryCreation.Destination.City,
		Street: deliveryCreation.Destination.Street,
		Number: deliveryCreation.Destination.Number,
	})

	if err != nil {
		return err
	}

	payload, _ := json.Marshal(&output.DeliveryDTO{
		Id:     deliveryCreation.Id,
		Service: service,
		Status: output.StatusSuccess,
		Location: &output.Location{
			Latitude:  geolocation.Latitude,
			Longitude: geolocation.Longitude,
		},
	})

	return h.publisher.Publish(ctx, payload)
}
