using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Validator for CancelSaleItemCommand
/// </summary>
public class CancelSaleItemValidator : AbstractValidator<CancelSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleItemValidator"/> class.
    /// </summary>
    public CancelSaleItemValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale Id is required");

        RuleFor(x => x.SaleItemId)
            .NotEmpty()
            .WithMessage("Sale Item Id is required");
    }
}