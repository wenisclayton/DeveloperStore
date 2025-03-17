using MediatR;
using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handler for the GetSaleCommand
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesCommand, GetSalesResultPaged>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSalesHandler"/> class.
    /// </summary>
    /// <param name="saleRepository"></param>
    /// <param name="mapper"></param>
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to get a sale
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<GetSalesResultPaged> Handle(GetSalesCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetSalesValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false)
            throw new ValidationException(validationResult.Errors);

        var pagedSales = await _saleRepository.GetSalesAsync(
            saleDate: request.SaleDate,
            pageSize: request.PageSize,
            branchId: request.BranchId,
            saleNumber: request.SaleNumber,
            customerId: request.CustomerId,
            pageNumber: request.PageNumber,
            cancellationToken: cancellationToken);

        if (pagedSales == null)
            throw new KeyNotFoundException("No sales found with the filters provided.");

        // Mapeia os registros para o DTO GetSalesResult e monta o objeto paginado.
        var resultPaged = new GetSalesResultPaged
        {
            Items = _mapper.Map<IEnumerable<GetSalesResult>>(pagedSales.Items),
            TotalCount = pagedSales.TotalCount,
            CurrentPage = pagedSales.CurrentPage,
            TotalPages = pagedSales.TotalPages
        };

        return resultPaged;
    }
}
