namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ChangeSale;


/// <summary>
/// Request to change a sale.
/// </summary>
public class ChangeSaleRequest
{
    /// <summary>
    /// Sale number.
    /// </summary>
    public int? SaleNumber { get; set; }
    
    /// <summary>
    /// Sale date.
    /// </summary>
    public DateTime? SaleDate { get; set; }

    /// <summary>
    /// Customer identifier.
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Branch identifier.
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// List of sale items.
    /// </summary>
    public List<ChangeSaleItemRequest>? Items { get; set; }
}