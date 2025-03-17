namespace Ambev.DeveloperEvaluation.RabbitSubscriber.IntegrationsEvents;

public class SaleCreatedIntegrationEvent : BaseIntegrationEvent
{
    public int SaleNumber { get; set; }
public string TipoEvent { get; set; }
    public DateTime DateEvent { get; set; }
}