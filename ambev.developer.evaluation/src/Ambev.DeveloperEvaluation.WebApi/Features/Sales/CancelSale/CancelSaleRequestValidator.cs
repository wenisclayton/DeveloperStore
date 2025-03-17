using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Validator for CancelSaleRequest
/// </summary>
public class CancelSaleRequestValidator : AbstractValidator<CancelSaleRequest>
{
    /// <summary>
    /// Initializes validation rules for CancelSaleRequest
    /// </summary>
    public CancelSaleRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("O ID da venda é obrigatório.");
    }
}