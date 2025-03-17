using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Profile for mapping the CancelSaleItemCommand to the CancelSaleItemResult
/// </summary>
 [ExcludeFromCodeCoverage]
public class CancelSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleItemProfile"/> class.
    /// </summary>
    public CancelSaleItemProfile()
    {
        CreateMap<Sale, CancelSaleItemResult>();
        CreateMap<SaleItem, CancelSaleItemResult>();
    }
}