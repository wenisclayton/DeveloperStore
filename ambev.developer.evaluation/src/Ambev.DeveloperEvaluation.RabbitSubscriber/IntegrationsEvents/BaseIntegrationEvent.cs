namespace Ambev.DeveloperEvaluation.RabbitSubscriber.IntegrationsEvents;

public class BaseIntegrationEvent
{
    public Guid Id { get; set; }

    public string TipoEvent { get; set; }
}