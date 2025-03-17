using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
RuleFor(x => x.SaleNumber).NotEmpty().WithMessage("Sale number is required.");
RuleFor(x => x.SaleDate).NotEmpty().WithMessage("Sale date is required.");
RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch ID is required.");
RuleForEach(x => x.Items).SetValidator(new CreateSaleItemRequestValidator());
    }
}