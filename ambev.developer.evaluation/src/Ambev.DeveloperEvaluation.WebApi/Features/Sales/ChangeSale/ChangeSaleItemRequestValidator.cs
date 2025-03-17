using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ChangeSale;

/// <summary>
/// Validation for each item in the sale.
/// </summary>
public class ChangeSaleItemRequestValidator : AbstractValidator<ChangeSaleItemRequest>
{

    /// <summary>
    /// Initializes validation rules for ChangeSaleItemRequest
    /// </summary>
    public ChangeSaleItemRequestValidator()
    {
        //RuleFor(x => x.ProductId)
        //    .NotEmpty()
        //    .WithMessage("Product Id is required.");

        //RuleFor(x => x.Quantity)
        //    .GreaterThan(0)
        //    .WithMessage("Quantity must be greater than zero.");

        //RuleFor(x => x.UnitPrice)
        //    .GreaterThanOrEqualTo(0)
        //    .WithMessage("Unit price cannot be negative.");
    }
}