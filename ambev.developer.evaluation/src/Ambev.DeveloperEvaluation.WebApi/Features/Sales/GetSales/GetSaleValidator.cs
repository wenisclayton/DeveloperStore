using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Validator for GetSaleRequest
/// </summary>
public class GetSaleValidator : AbstractValidator<GetSalesRequest>
{
    /// <summary>
    /// Initializes validation rules for GetSaleRequest
    /// </summary>
    public GetSaleValidator()
    {
        //RuleFor(x => x.Id).NotEmpty().WithMessage("Client ID is required.");
    }
}