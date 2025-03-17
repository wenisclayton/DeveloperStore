using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Validator for GetSaleCommand
/// </summary>
public class GetSalesValidator : AbstractValidator<GetSalesCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSalesValidator"/> class.
    /// </summary>
    public GetSalesValidator()
    {

    }
}
