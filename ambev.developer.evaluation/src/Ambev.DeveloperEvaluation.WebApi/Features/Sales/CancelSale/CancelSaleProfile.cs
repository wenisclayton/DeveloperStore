using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;


/// <summary>
/// AutoMapper profile for cancel sale-related mappings
/// </summary>
public class CancelSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleProfile"/> class
    /// </summary>
    public CancelSaleProfile()
    {
        
        CreateMap<CancelSaleRequest, CancelSaleCommand>();
        CreateMap<CancelSaleResult, CancelSaleResponse>();
    }
}