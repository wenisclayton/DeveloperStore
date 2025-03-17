using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Command to get a sale.
/// </summary>
public record GetSaleCommand : IRequest<GetSaleResult>
{
    /// <summary>
    /// Gets the ID of the sale to retrieve
    /// </summary>
    public Guid Id { get; set; }
}
