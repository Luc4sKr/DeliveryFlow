package output

type Status string

const (
	StatusSuccess = "SUCCESS"
	StatusFailed = "FAILED"
)

type DeliveryDTO struct {
	Id string
	Service string
	Status Status
	Priority int
}
