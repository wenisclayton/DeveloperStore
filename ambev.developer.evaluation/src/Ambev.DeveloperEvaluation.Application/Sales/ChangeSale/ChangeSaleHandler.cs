using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;
using Ambev.DeveloperEvaluation.Domain.Events.SaleAggregate.Notifications;
using Ambev.DeveloperEvaluation.Domain.Entities.AuditLog;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;


/// <summary>
/// Handler for the ChangeSaleCommand
/// </summary>
public class ChangeSaleHandler : IRequestHandler<ChangeSaleCommand, ChangeSaleResult>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IDomainEventStore _eventStore;
    private readonly ISaleRepository _saleRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;


    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeSaleHandler"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="mediator">The Mediator instance</param>
    /// <param name="eventStore">The Event Store instance</param>
    /// <param name="saleRepository">sale repository</param>
    /// <param name="branchRepository">branch repository</param>
    /// <param name="productRepository">product repository</param>
    /// <param name="customerRepository">customer repository</param>
    public ChangeSaleHandler(
        IMapper mapper,
        IMediator mediator,
        IDomainEventStore eventStore,
        ISaleRepository saleRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository
        )
    {
        _mapper = mapper;
        _mediator = mediator;
        _eventStore = eventStore;
        _saleRepository = saleRepository;
        _branchRepository = branchRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Handles the request to change a sale
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<ChangeSaleResult> Handle(ChangeSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
        {
            throw new InvalidOperationException($"Venda com id {command.Id} não encontrada.");
        }

        var customer = command.CustomerId.HasValue 
            ? await _customerRepository.GetByIdAsync(command.CustomerId!.Value, cancellationToken) 
            : null ;
        
        if (customer == null)
        {
            throw new InvalidOperationException($"Cliente com id {command.CustomerId} não encontrado.");
        }

        var branch = command.BranchId.HasValue
            ? await _branchRepository.GetByIdAsync(command.BranchId.Value, cancellationToken)
            : null;
        
        if (branch == null)
        {
            throw new InvalidOperationException($"Filial com id {command.BranchId} não encontrada.");
        }

        sale.Change(command.SaleNumber, command.SaleDate, customer, branch);

        if (command.Items?.Count > 0)
        {
            var newSaleItems = new List<(Product product, int? quantity, decimal? unitPrice)>();
            foreach (var itemCommand in command.Items)
            {
                var product = itemCommand.ProductId.HasValue 
                    ? await _productRepository.GetByIdAsync(itemCommand.ProductId.Value, cancellationToken) 
                    : null;
                
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with id {itemCommand.ProductId} not found.");
                }
                newSaleItems.Add((product, itemCommand.Quantity, itemCommand.UnitPrice));
            }

            sale.ChangeSaleItems(newSaleItems);
        }
        
        await _saleRepository.UpdateAsync(sale, cancellationToken);
        await _mediator.Publish(new SaleChangedEvent(sale.Id, sale.SaleNumber, sale.SaleDate), cancellationToken);
        await SaveEventLog(sale, cancellationToken);
        return _mapper.Map<ChangeSaleResult>(sale);
    }

    private async Task SaveEventLog(Sale createdSale, CancellationToken cancellationToken)
    {
        var auditEvent = new AuditEvent<Sale>(
            aggregateId: createdSale.Id,
            eventType: AuditEventType.Changed,
            data: createdSale
        );
        await _eventStore.SaveEventAsync(auditEvent, cancellationToken);
    }
}