namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

/// <summary>
/// Represents the request to cancel a sale item
/// </summary>
public class CancelSaleItemRequest
{

    /// <summary>
    /// Gets or sets the sale id
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the sale item id
    /// </summary>
    public Guid SaleItemId { get; set; }
}