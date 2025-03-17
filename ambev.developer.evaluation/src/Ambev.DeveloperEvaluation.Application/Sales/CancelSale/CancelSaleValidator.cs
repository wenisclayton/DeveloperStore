using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Validator for CancelSaleCommand
/// </summary>
public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleValidator"/> class.
    /// </summary>
    public CancelSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}