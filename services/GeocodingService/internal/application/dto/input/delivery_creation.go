package input

type DeliveryCreationDTO struct {
	Id          string
	Destination struct {
		Street string
		Number string
		City   string
		State  string
	}
}
