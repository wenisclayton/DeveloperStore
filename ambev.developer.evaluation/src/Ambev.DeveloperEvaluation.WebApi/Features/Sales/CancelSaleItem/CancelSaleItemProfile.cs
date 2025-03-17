using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

/// <summary>
/// AutoMapper profile for cancel sale item-related mappings
/// </summary>
public class CancelSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleItemProfile"/> class
    /// </summary>
    public CancelSaleItemProfile()
    {
                CreateMap<CancelSaleItemRequest, CancelSaleItemCommand>();
                CreateMap<CancelSaleItemResult, CancelSaleItemResponse>();
    }
}