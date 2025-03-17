﻿namespace Ambev.DeveloperEvaluation.Domain.Events.SaleAggregate.Notifications;

/// <summary>
/// Represents the event that is raised when a sale is Modified Event.
/// </summary>
/// <param name="id"></param>
/// <param name="saleNumber"></param>
/// <param name="dateEvent"></param>
public class SaleChangedEvent(Guid id, int saleNumber, DateTime dateEvent) : SaleEvent(id, saleNumber, dateEvent)
{
    public string TipoEvent = "Sale Modified";
}