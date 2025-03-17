using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Command to cancel a sale item
/// </summary>
public class CancelSaleItemCommand : IRequest<CancelSaleItemResult>
{
    /// <summary>
    /// Sale ID for cancel
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Sale Item ID for cancel
    /// </summary>
    public Guid SaleItemId { get; set; }
}