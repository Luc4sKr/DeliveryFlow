package output

type Location struct {
	Latitude  float64
	Longitude float64
}

type Status string

const (
	StatusSuccess = "SUCCESS"
	StatusError   = "ERROR"
)

type GeoLocation struct {
	Id       string
	Status   Status
	Location *Location
}
