using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// AutoMapper profile for create sale-related mappings
/// </summary>
public class CreateSaleItemProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleItemProfile"/> class
    /// </summary>
    public CreateSaleItemProfile()
    {
        CreateMap<CreateSaleItemRequest, CreateSaleItemCommand>();
    }
}