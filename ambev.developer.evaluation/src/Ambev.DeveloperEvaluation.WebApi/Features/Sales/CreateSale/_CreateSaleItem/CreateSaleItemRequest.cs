namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents the request to create a sale item
/// </summary>
public class CreateSaleItemRequest
{
    /// <summary>
    /// The ID of the product to add to the sale
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// The quantity of the product to add to the sale
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }
}