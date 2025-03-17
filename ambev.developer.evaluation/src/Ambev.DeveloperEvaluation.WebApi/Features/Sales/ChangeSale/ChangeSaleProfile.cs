using Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ChangeSale;

/// <summary>
/// AutoMapper profile for change sale-related mappings
/// </summary>
public class ChangeSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeSaleProfile"/> class
    /// </summary>
    public ChangeSaleProfile()
    {

        CreateMap<ChangeSaleRequest, ChangeSaleCommand>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<ChangeSaleItemRequest, ChangeSaleItemCommand>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<ChangeSaleResult, ChangeSaleResponse>();
    }
}