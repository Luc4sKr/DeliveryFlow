package geocoding

import (
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"github.com/Luc4sKr/DeliveryFlow/services/GeocodingService/internal/application/ports"
	"github.com/Luc4sKr/DeliveryFlow/services/GeocodingService/internal/domain"
	"net/http"
	"net/url"
	"strconv"
)

var ErrInvalidAPIResponse = errors.New("invalid API response")
var ErrNoLocationFound = errors.New("no location found")
var ErrInvalidDataFormat = errors.New("invalid data format")

type locationResponse struct {
	Latitude  string `json:"lat"`
	Longitude string `json:"lon"`
}

type OpenStreetMapsGeoCoder struct {
	client *http.Client
}

func NewGeoCoder(client *http.Client) *OpenStreetMapsGeoCoder {
	return &OpenStreetMapsGeoCoder{
		client: client,
	}
}

func (g *OpenStreetMapsGeoCoder) EncodeAddress(address *domain.Address) (*domain.GeoLocation, error) {
	query := url.Values{}

	query.Set("q", fmt.Sprintf("%s,%s,%s,%s", address.Street, address.Number, address.City, address.State))
	query.Set("format", "jsonv2")
	query.Set("limit", "1")

	url := url.URL{
		Scheme:   "https",
		Host:     "nominatim.openstreetmap.org",
		Path:     "/search",
		RawQuery: query.Encode(),
	}

	req, _ := http.NewRequestWithContext(context.Background(), http.MethodGet, url.String(), nil)
	req.Header.Set("User-Agent", "Mozilla/5.0 (X11; Linux x86_64; rv:149.0) Gecko/20100101 Firefox/149.0")

	response, err := g.client.Do(req)
	if err != nil {
		return nil, err
	}
	defer response.Body.Close()

	if response.StatusCode != http.StatusOK {
		return nil, fmt.Errorf("%w: api responded with %d status code", ErrInvalidAPIResponse, response.StatusCode)
	}

	var data []locationResponse

	decoder := json.NewDecoder(response.Body)
	if err := decoder.Decode(&data); err != nil {
		return nil, err
	}

	if len(data) == 0 {
		return nil, ErrNoLocationFound
	}
	location := data[0]
	lat, err := strconv.ParseFloat(location.Latitude, 64)
	if err != nil {
		return nil, fmt.Errorf("%w: latitude %s is not a valid float", location.Latitude)
	}
	lon, err := strconv.ParseFloat(location.Longitude, 64)
	if err != nil {
		return nil, fmt.Errorf("%w: longitude %s is not a valid float", location.Longitude)
	}
	return &domain.GeoLocation{
		Latitude:  lat,
		Longitude: lon,
	}, nil
}

var _ ports.GeoCoder = (*OpenStreetMapsGeoCoder)(nil)
