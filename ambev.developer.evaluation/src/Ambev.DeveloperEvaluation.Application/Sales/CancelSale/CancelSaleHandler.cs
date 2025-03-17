using MediatR;
using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities.AuditLog;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;
using Ambev.DeveloperEvaluation.Domain.Events.SaleAggregate.Notifications;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for the CancelSaleCommand
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IDomainEventStore _eventStore;
    private readonly ISaleRepository _saleRepository;


    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleHandler"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="mediator">The Mediator instance</param>
    /// <param name="eventStore">The Event Store instance</param>
    /// <param name="saleRepository"></param>
    public CancelSaleHandler(IMapper mapper, IMediator mediator, IDomainEventStore eventStore, ISaleRepository saleRepository)
    {
        _mapper = mapper;
        _mediator = mediator;
        _eventStore = eventStore;
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Handles the request to cancel a sale
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"User with ID {request.Id} not found");

        sale.Cancel();
        await _saleRepository.SaveChangesAsync(cancellationToken);
        await _mediator.Publish(new SaleCancelledEvent(sale.Id, sale.SaleNumber, sale.SaleDate), cancellationToken);
        await SaveEventLog(sale, cancellationToken);
        return _mapper.Map<CancelSaleResult>(sale);
    }

    /// <summary>
    /// Saves the event log
    /// </summary>
    /// <param name="createdSale">created sale</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>The task</returns>
    private async Task SaveEventLog(Sale createdSale, CancellationToken cancellationToken)
    {
        var auditEvent = new AuditEvent<Sale>(
            aggregateId: createdSale.Id,
            eventType: AuditEventType.Canceled,
            data: createdSale
        );
        await _eventStore.SaveEventAsync(auditEvent, cancellationToken);
    }
}