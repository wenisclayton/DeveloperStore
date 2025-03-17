using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// AutoMapper profile for get sale-related mappings
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleProfile"/> class
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<GetSaleResult, GetSaleResponse>();
        CreateMap<GetSaleRequest, GetSaleCommand>();
    }
}