namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Represents the request to cancel a sale
/// </summary>
public class CancelSaleRequest
{
    /// <summary>
    /// The ID of the sale to cancel
    /// </summary>
    public Guid Id { get; set; }
}