package ports

import "github.com/Luc4sKr/DeliveryFlow/services/GeocodingService/internal/domain"

type GeoCoder interface {
	EncodeAddress(*domain.Address) (*domain.GeoLocation, error)
}
