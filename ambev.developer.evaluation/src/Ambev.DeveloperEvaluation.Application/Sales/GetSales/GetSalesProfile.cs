using System.Diagnostics.CodeAnalysis;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Profile for mapping the Sale to the GetSaleResult and SaleItem to the GetSaleItemResult
/// </summary>
[ExcludeFromCodeCoverage]
public class GetSalesProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSalesProfile"/> class.
    /// </summary>
    public GetSalesProfile()
    {
        CreateMap<Sale, GetSalesResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Branch.Id))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalValue));

        CreateMap<SaleItem, GetSalesItemResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
            .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalValue));
    }
}
