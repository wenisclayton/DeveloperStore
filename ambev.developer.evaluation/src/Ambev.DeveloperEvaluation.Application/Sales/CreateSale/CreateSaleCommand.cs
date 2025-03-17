using MediatR;
using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command to create a sale.
/// </summary>
public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public int SaleNumber { get; set; } = int.MinValue;

    /// <summary>
    /// Gets or sets the date of the sale
    /// </summary>
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the customer Id
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch Id
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the list of sale items
    /// </summary>
    public List<CreateSaleItemCommand> Items { get; set; }

    /// <summary>
    /// Validates the command
    /// </summary>
    /// <returns></returns>
    public ValidationResultDetail Validate()
    {
        var validator = new CreateSaleCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}