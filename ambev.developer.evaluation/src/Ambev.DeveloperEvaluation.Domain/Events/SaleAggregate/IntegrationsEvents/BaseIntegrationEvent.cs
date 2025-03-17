namespace Ambev.DeveloperEvaluation.Domain.Events.SaleAggregate.IntegrationEvent;

public class BaseIntegrationEvent
{
    public Guid Id { get; set; }

    public string TipoEvent { get; set; }
}