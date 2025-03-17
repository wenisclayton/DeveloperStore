using EasyNetQ;
using Ambev.DeveloperEvaluation.Integration.Interfaces;

namespace Ambev.DeveloperEvaluation.Integration.Adapters.Messaging.RabbitMQ;

/// <summary>
/// Publishes messages to RabbitMQ.
/// </summary>
/// <summary>
/// Publishes messages to RabbitMQ using EasyNetQ.
/// </summary>
public class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly IBus _bus;

    /// <summary>
    /// Initializes a new instance of RabbitMQPublisher.
    /// It uses the RabbitMQConnectionFactory to create the bus.
    /// </summary>
    /// <param name="connectionFactory">The RabbitMQ connection factory using EasyNetQ.</param>
    public RabbitMQPublisher(RabbitMQConnectionFactory connectionFactory)
    {
        _bus = connectionFactory.CreateBus();
    }

    /// <summary>
    /// Publishes the specified event/message to RabbitMQ.
    /// </summary>
    /// <typeparam name="T">The type of the event/message.</typeparam>
    /// <param name="eventMessage">The event/message to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task PublishAsync<T>(T eventMessage, CancellationToken cancellationToken) where T : class
    {
        await _bus.PubSub.PublishAsync(eventMessage, cancellationToken);
    }
}