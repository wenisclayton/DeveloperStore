using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Data Transfer Object (DTO) for returning sale information.
/// </summary>
public class GetSaleResult
{
    /// <summary>
    /// Gets or sets the sale Id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public int SaleNumber { get; set; }

    /// <summary>
    /// Gets or sets the date of the sale.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer Id.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the branch id.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string BranchName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the sale is canceled.
    /// </summary>
    public bool IsCanceled { get; set; }

    /// <summary>
    /// Gets or sets the list of sale items.
    /// </summary>
    public IReadOnlyCollection<GetSaleItemResult> Items { get; set; }

    /// <summary>
    /// Gets or sets the total value of the sale.
    /// </summary>
    public decimal TotalValue { get; set; }
}
