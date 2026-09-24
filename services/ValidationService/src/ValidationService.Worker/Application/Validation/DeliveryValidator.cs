using ValidationService.Worker.Contracts.Events;

namespace ValidationService.Worker.Application.Validation;

public class DeliveryValidator
{
    public IReadOnlyCollection<string> Validate(DeliveryCreatedEvent delivery)
    {
        var errors = new List<string>();
        var address = delivery.Destination;

        if (delivery.Id == Guid.Empty)
            errors.Add("O identificador da entrega e obrigatorio.");
        if (delivery.Weight <= 0)
            errors.Add("O peso deve ser maior que zero.");
        if (delivery.Volume <= 0)
            errors.Add("O volume deve ser maior que zero.");
        if (string.IsNullOrWhiteSpace(address.Street))
            errors.Add("A rua do destino e obrigatoria.");
        if (string.IsNullOrWhiteSpace(address.Number))
            errors.Add("O numero do destino e obrigatorio.");
        if (string.IsNullOrWhiteSpace(address.City))
            errors.Add("A cidade do destino e obrigatoria.");
        if (string.IsNullOrWhiteSpace(address.State))
            errors.Add("O estado do destino e obrigatorio.");
        if (string.IsNullOrWhiteSpace(address.ZipCode))
            errors.Add("O CEP do destino e obrigatorio.");
        if (string.IsNullOrWhiteSpace(address.Country))
            errors.Add("O pais do destino e obrigatorio.");

        return errors;
    }
}