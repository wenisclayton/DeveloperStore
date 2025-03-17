using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;


/// <summary>
/// Command to change a sale.
/// </summary>
public class ChangeSaleCommand : IRequest<ChangeSaleResult>
{
    /// <summary>
    /// Gets or sets the sale Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public int? SaleNumber { get; set; }

    /// <summary>
    /// Gets or sets the date of the sale
    /// </summary>
    public DateTime? SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer Id
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch Id
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Gets or sets the list of sale items
    /// </summary>
    public List<ChangeSaleItemCommand>? Items { get; set; }
}