using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.SaleAggregate.Notifications;

public class SaleEvent(Guid id, int saleNumber, DateTime dateEvent) : INotification
{
    public Guid Id { get; } = id;
    public int SaleNumber { get; } = saleNumber;
    public DateTime DateEvent { get; } = dateEvent;
}