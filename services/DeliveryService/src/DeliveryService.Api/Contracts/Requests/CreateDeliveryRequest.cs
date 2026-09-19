using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Api.Contracts.Requests;

public class CreateDeliveryRequest
{
    [Required]
    public AddressRequest Destination { get; set; } = new();

    [Range(0.01, double.MaxValue)]
    public decimal Weight { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Volume { get; set; }
}

public class AddressRequest
{
    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public string Number { get; set; } = string.Empty;

    public string Complement { get; set; } = string.Empty;

    [Required]
    public string Neighborhood { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    public string ZipCode { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;
}