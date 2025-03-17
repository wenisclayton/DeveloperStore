using MediatR;
using Ambev.DeveloperEvaluation.Integration.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Events.SaleAggregate.Notifications;
using Ambev.DeveloperEvaluation.Domain.Events.SaleAggregate.IntegrationEvent;

namespace Ambev.DeveloperEvaluation.Integration.Events.Handlers;

public class SaleCreatedIntegrationEventHandler(IRabbitMQPublisher rabbitMqPublisher)
    : INotificationHandler<SaleCreatedEvent>
{
    public async Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new SaleCreatedIntegrationEvent
        {
            Id = notification.Id,
            DateEvent = notification.DateEvent,
            SaleNumber = notification.SaleNumber,
            TipoEvent = notification.TipoEvent
        };

        await rabbitMqPublisher.PublishAsync(integrationEvent, cancellationToken);
    }
}