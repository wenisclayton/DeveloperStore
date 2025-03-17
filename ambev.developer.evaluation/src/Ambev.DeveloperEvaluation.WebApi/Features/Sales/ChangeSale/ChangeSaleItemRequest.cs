namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ChangeSale;

/// <summary>
/// Request to change a sale item.
/// </summary>
public class ChangeSaleItemRequest
{
    /// <summary>
    /// Product identifier.
    /// </summary>
    public Guid? ProductId { get; set; }

    /// <summary>
    /// Product quantity.
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// Product unit price.
    /// </summary>
    public decimal? UnitPrice { get; set; }
}