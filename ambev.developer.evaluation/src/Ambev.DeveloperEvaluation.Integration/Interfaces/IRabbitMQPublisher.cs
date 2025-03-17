namespace Ambev.DeveloperEvaluation.Integration.Interfaces;

public interface IRabbitMQPublisher
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken) where T : class;
}