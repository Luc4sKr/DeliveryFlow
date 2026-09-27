package output

type Location struct {
	Latitude  float64
	Longitude float64
}

type Status string

const (
	StatusSuccess = "SUCCESS"
	StatusFailed  = "FAILED"
)

type DeliveryDTO struct {
	Id       string
	Service string
	Status   Status
	Location *Location
}
