﻿namespace Ambev.DeveloperEvaluation.Domain.Events.SaleAggregate.IntegrationEvent;

public class SaleCancelledIntegrationEvent : BaseIntegrationEvent
{
    public int SaleNumber { get; set; }
    public string TipoEvent { get; set; }
    public DateTime DateEvent { get; set; }
}