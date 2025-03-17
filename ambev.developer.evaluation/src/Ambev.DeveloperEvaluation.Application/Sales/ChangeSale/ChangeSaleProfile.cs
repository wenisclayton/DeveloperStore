using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;

/// <summary>
/// Profile for mapping the ChangeSaleCommand to the ChangeSaleResult
/// </summary>
[ExcludeFromCodeCoverage]
public class ChangeSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeSaleProfile"/> class.
    /// </summary>
    public ChangeSaleProfile()
    {
        CreateMap<ChangeSaleCommand, Sale>()
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Branch, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<Sale, ChangeSaleResult>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Branch.Id))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalValue));

        CreateMap<SaleItem, ChangeSaleItemResult>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
            .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalValue));
    }
}