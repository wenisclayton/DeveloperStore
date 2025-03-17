using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ChangeSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sale operations
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of SalesController.
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }


    /// <summary>
    /// Creates a new sale.
    /// </summary>
    /// <param name="request">The sale creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    [HttpPost(Name = "CreateSale")]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new CreateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created("CreateSale", new { id = response.Id }, _mapper.Map<CreateSaleResponse>(response));
    }

    /// <summary>
    /// Retrieves a sale by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale details if found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetSaleRequest { Id = id };
        var validator = new GetSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid is false)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<GetSaleResponse>(response));
    }

    /// <summary>
    /// Retrieves a paginated list of sales filtered by SaleDate, CustomerId and BranchId.
    /// </summary>
    /// <param name="request">The sales query request containing filter parameters and pagination info</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated response with the list of sales</returns>
    [HttpGet("sales")]
    [ProducesResponseType(typeof(PaginatedResponse<GetSalesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSales([FromQuery] GetSalesRequest request, CancellationToken cancellationToken)
    {
        var validator = new GetSalesRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid is false)
            return BadRequest(validationResult.Errors);
        
        var command = _mapper.Map<GetSalesCommand>(request);
        var paginatedSales = await _mediator.Send(command, cancellationToken);
        var salesResponse = _mapper.Map<List<GetSalesResponse>>(paginatedSales.Items);

        var paginatedResponseList = new PaginatedList<GetSalesResponse>(
            salesResponse,
            paginatedSales.TotalCount,
            paginatedSales.CurrentPage,
            request.PageSize
        );
        return OkPaginated(paginatedResponseList);
    }

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="request">The change sale request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<ChangeSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeSale([FromRoute] Guid id, [FromBody] ChangeSaleRequest request,
        CancellationToken cancellationToken)
    {

        var validator = new ChangeSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<ChangeSaleCommand>(request);
        command.Id = id;
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(message: "Sale updated successfully", data: _mapper.Map<ChangeSaleResponse>(response));
    }


    /// <summary>
    /// Cancela uma venda.
    /// </summary>
    /// <param name="id">O identificador único da venda</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Confirmação de que a venda foi cancelada</returns>
    [HttpPost("{id}/cancel", Name = "CancelSale")]
    [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new CancelSaleRequest { Id = id };
        var validator = new CancelSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) 
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CancelSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(message: "Sale cancelled successfully", data: _mapper.Map<CancelSaleResponse>(response));
    }

    /// <summary>
    /// Cancels a sale item.
    /// </summary>
    /// <param name="id">The unique identifier of the sale.</param>
    /// <param name="itemId">The unique identifier of the sale item.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Confirmation that the sale item has been cancelled.</returns>
    [HttpPost("{id}/items/{itemId}/cancel", Name = "CancelSaleItem")]
    [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSaleItem([FromRoute] Guid id, [FromRoute] Guid itemId, CancellationToken cancellationToken)
    {
        var request = new CancelSaleItemRequest { SaleId = id, SaleItemId = itemId };
        var validator = new CancelSaleItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid  is false)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CancelSaleItemCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(message : "Sale item cancelled successfully", data: _mapper.Map<CancelSaleItemResponse>(response));
    }
}