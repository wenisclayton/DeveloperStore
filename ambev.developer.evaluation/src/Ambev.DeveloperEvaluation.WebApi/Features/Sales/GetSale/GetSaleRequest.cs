namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Represents the request to retrieve a sale
/// </summary>
public class GetSaleRequest
{
    /// <summary>
    /// The ID of the sale to retrieve
    /// </summary>
    public Guid Id { get; set; }
}