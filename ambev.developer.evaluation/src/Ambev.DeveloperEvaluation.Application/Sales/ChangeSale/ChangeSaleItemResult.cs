namespace Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;

public class ChangeSaleItemResult
{
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
}