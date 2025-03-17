using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Profile for mapping the CancelSaleCommand to the CancelSaleResult
/// </summary>
public class CancelSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleProfile"/> class.
    /// </summary>
    public CancelSaleProfile()
    {
        CreateMap<Sale, CancelSaleResult>();
    }
}