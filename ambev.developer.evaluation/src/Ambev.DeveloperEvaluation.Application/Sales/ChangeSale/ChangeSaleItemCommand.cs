namespace Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;

/// <summary>
/// Command to change a sale.
/// </summary>
public class ChangeSaleItemCommand
{
    /// <summary>
    /// Gets or sets the product Id
    /// </summary>
    public Guid? ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price
    /// </summary>
    public decimal? UnitPrice { get; set; }
}
