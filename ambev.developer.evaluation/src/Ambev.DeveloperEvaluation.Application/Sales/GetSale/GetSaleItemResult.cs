namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Data Transfer Object (DTO) for returning sale item details.
/// </summary>
public class GetSaleItemResult
{
    /// <summary>
    /// Gets or sets the sale Item Id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount applied to the sale item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total value for the sale item.
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the sale item is canceled.
    /// </summary>
    public bool IsCanceled { get; set; }
}