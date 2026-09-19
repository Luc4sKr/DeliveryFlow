using Microsoft.AspNetCore.Mvc;
using DeliveryService.Api.Contracts.Requests;
using DeliveryService.Api.Contracts.Responses;
using DeliveryService.Api.Application.Deliveries;

namespace DeliveryService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryController(
    CreateDeliveryUseCase createDelivery,
    GetDeliveryByIdUseCase getDeliveryById) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(DeliveryResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<DeliveryResponse>> Create(
        CreateDeliveryRequest request,
        CancellationToken cancellationToken)
    {
        var delivery = await createDelivery.ExecuteAsync(new CreateDeliveryCommand(
            request.Destination.Street,
            request.Destination.Number,
            request.Destination.Complement,
            request.Destination.Neighborhood,
            request.Destination.City,
            request.Destination.State,
            request.Destination.ZipCode,
            request.Destination.Country,
            request.Weight,
            request.Volume), cancellationToken);
        var response = DeliveryResponse.FromDomain(delivery);

        return CreatedAtAction(nameof(GetById), new { id = delivery.Id }, response);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<DeliveryResponse> GetById(Guid id)
    {
        var delivery = getDeliveryById.Execute(id);
        return delivery is null ? NotFound() : Ok(DeliveryResponse.FromDomain(delivery));
    }
}
