using Ambev.DeveloperEvaluation.Domain.Entities.AuditLog;
using MediatR;
using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;
using Ambev.DeveloperEvaluation.Domain.Events.SaleAggregate.Notifications;
using Ambev.DeveloperEvaluation.Domain.Enums;


namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for the CreateSaleCommand
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IDomainEventStore _eventStore;
    private readonly ISaleRepository _saleRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<CreateSaleCommand> _validator;


    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="mediator">The Mediator instance</param>
    /// <param name="eventStore">The Event Store instance</param>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="productRepository">The product repository</param>
    /// <param name="customerRepository">The customer repository</param>
    public CreateSaleHandler(
            IMapper mapper,
            IMediator mediator,
            IDomainEventStore eventStore,
            ISaleRepository saleRepository,
            IBranchRepository branchRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _mediator = mediator;
            _eventStore = eventStore;
        _saleRepository = saleRepository;
            _branchRepository = branchRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _validator = new CreateSaleCommandValidator();
        }


    /// <summary>
    /// Handles the CreateSaleHandler request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    /// <exception cref="ValidationException">Validation</exception>
    /// <exception cref="InvalidOperationException">Operation Invalid</exception>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid is false)
                throw new ValidationException(validationResult.Errors);

            var existingSale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
            if (existingSale is not null)
                throw new InvalidOperationException($"Sale with sale number {command.SaleNumber} already exists");

            
            var customer = await GetCustomerByIdAsync(command.CustomerId, cancellationToken);
            var branch = await GetBranchByIdAsync(command.BranchId, cancellationToken);

            var sale = new Sale(command.SaleNumber, command.SaleDate, customer, branch);

            
            foreach (var item in command.Items)
            {
                var product = await GetProductByIdAsync(item.ProductId, cancellationToken);
                sale.AddSaleItem(product, item.Quantity, item.UnitPrice);
            }

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
            await PublishMessageEvent(cancellationToken, createdSale);
            await SaveEventLog(createdSale, cancellationToken);
        return _mapper.Map<CreateSaleResult>(createdSale);
        }

    private async Task SaveEventLog(Sale createdSale, CancellationToken cancellationToken)
    {
        var auditEvent = new AuditEvent<Sale>(
            aggregateId: createdSale.Id,
            eventType: AuditEventType.Created,
            data: createdSale
        );
        await _eventStore.SaveEventAsync(auditEvent, cancellationToken);
    }



    private async Task PublishMessageEvent(CancellationToken cancellationToken, Sale createdSale) => 
        await _mediator.Publish(new SaleCreatedEvent(createdSale.Id, createdSale.SaleNumber, createdSale.SaleDate), cancellationToken);
    

    private async Task<Customer> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null)
            throw new InvalidOperationException($"Customer with ID {customerId} was not found");
        return customer;
    }

    private async Task<Branch> GetBranchByIdAsync(Guid branchId, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(branchId, cancellationToken);
        if (branch is null)
            throw new InvalidOperationException($"Branch with ID {branchId} was not found");
        return branch;
    }

    private async Task<Product> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
            throw new InvalidOperationException($"Product with ID {productId} was not found");
        return product;
    }
}