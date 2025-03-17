using Ambev.DeveloperEvaluation.Domain.Events.SaleAggregate.IntegrationEvent;
using Ambev.DeveloperEvaluation.Domain.Events.SaleAggregate.Notifications;
using Ambev.DeveloperEvaluation.Integration.Interfaces;
using MediatR;

namespace Ambev.DeveloperEvaluation.Integration.Events.Handlers;

public class SaleCancelledIntegrationEventHandler(IRabbitMQPublisher rabbitMqPublisher)
    : INotificationHandler<SaleCancelledEvent>
{
    public async Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new SaleModifiedIntegrationEvent
        {
            Id = notification.Id,
            DateEvent = notification.DateEvent,
            SaleNumber = notification.SaleNumber,
            TipoEvent = notification.TipoEvent
        };

        await rabbitMqPublisher.PublishAsync(integrationEvent, cancellationToken);
    }
}