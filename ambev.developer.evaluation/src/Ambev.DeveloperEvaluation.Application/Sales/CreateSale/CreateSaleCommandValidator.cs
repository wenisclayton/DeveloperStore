using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;


/// <summary>
/// Validator for CreateSaleCommand
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleCommandValidator"/> class.
    /// </summary>
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.SaleNumber)
            .NotEmpty()
            .WithMessage("Sales Number is required");

        RuleFor(x => x.SaleDate)
            .NotEmpty()
            .WithMessage("Sales Date is required");

        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one sale item is required");

        RuleForEach(x => x.Items)
            .SetValidator(new CreateSaleItemCommandValidator());
    }
}