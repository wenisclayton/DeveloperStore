using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// AutoMapper profile for get sale-related mappings
/// </summary>
public class GetSalesProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSalesProfile"/> class
    /// </summary>
    public GetSalesProfile()
    {
        CreateMap<GetSalesRequest, GetSalesCommand>();

        CreateMap<GetSalesResult, GetSalesResponse>();

        CreateMap<GetSalesItemResult, GetSaleItemResult>();
    }
}