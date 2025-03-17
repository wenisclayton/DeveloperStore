using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Profile for mapping the CreateSaleCommand to the CreateSaleResult
/// </summary>
[ExcludeFromCodeCoverage]
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleProfile"/> class.
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Sale>()
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Branch, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore());
        
        CreateMap<Sale, CreateSaleResult>();
    }

}
