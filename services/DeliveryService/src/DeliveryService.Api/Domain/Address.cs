using DeliveryService.Api.Domain.Common;

namespace DeliveryService.Api.Domain
{
    public class Address : BaseEntity
    {
        public string Street { get; private set; } = string.Empty;
        public string Number { get; private set; } = string.Empty;
        public string Complement { get; private set; } = string.Empty;
        public string Neighborhood { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;
        public string ZipCode { get; private set; } = string.Empty;
        public string Country { get; private set; } = string.Empty;

        private Address() { }

        public Address(string street, string number, string complement, string neighborhood, string city, string state, string zipCode, string country)
        {
            Street = street;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZipCode = zipCode;
            Country = country;
        }
    }
}
