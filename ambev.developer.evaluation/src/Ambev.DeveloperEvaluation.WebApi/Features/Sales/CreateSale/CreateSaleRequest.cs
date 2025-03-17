namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequest
{
    /// <summary>
    /// The number of the sale
    /// </summary>
    public string SaleNumber { get; set; }

    /// <summary>
    /// The date of the sale
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The ID of the customer that made the sale
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// The ID of the branch where the sale was made
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// The items of the sale
    /// </summary>
    public List<CreateSaleItemRequest> Items { get; set; }
}