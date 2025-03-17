namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Represents the response to cancel a sale
/// </summary>
public class CancelSaleResponse
{
    /// <summary>
    /// The ID of the sale that was canceled
    /// </summary>
    public Guid Id { get; set; }
}