using Bogus;
using Xunit;
using MediatR;
using AutoMapper;
using NSubstitute;
using System.Reflection;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CreateSaleHandlerTests
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly CreateSaleHandler _handler;
    private readonly IDomainEventStore _eventStore;
    private readonly ISaleRepository _saleRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly Faker<CreateSaleCommand> _commandFaker;
    private readonly ICustomerRepository _customerRepository;

    public CreateSaleHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediator>();
        _eventStore = Substitute.For<IDomainEventStore>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();

        _handler = new CreateSaleHandler(
            mapper: _mapper,
            mediator: _mediator,
            eventStore: _eventStore,
            saleRepository: _saleRepository,
            branchRepository: _branchRepository,
            productRepository: _productRepository,
            customerRepository: _customerRepository);

        _commandFaker = new Faker<CreateSaleCommand>()
            .RuleFor(x => x.SaleNumber, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.SaleDate, f => f.Date.Recent())
            .RuleFor(x => x.CustomerId, f => f.Random.Guid())
            .RuleFor(x => x.BranchId, f => f.Random.Guid())
            .RuleFor(x => x.Items, f => new List<CreateSaleItemCommand> {
                new()
                {
                    ProductId = f.Random.Guid(),
                    Quantity = f.Random.Int(1, 5),
                    UnitPrice = f.Random.Decimal(1, 100),
                    Discount = 0m
                }
            });
    }

    [Fact]
    public async Task Handle_ComandoValido_RetornaCreateSaleResult()
    {
var command = _commandFaker.Generate();
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult<Sale>(null!));

        var customer = new Customer("Cliente Teste");
        var branch = new Branch("Filial Teste");

        _customerRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(customer));
        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(branch));
        foreach (var item in command.Items)
        {
            var prod = new Product("Produto Teste", 10m);
            typeof(Product)
                .GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!
                .SetValue(prod, item.ProductId);

            _productRepository.GetByIdAsync(item.ProductId, Arg.Any<CancellationToken>())!
                .Returns(Task.FromResult(prod));
        }

        // Simular criação da venda
        var sale = new Sale(command.SaleNumber, command.SaleDate, customer, branch);
        foreach (var item in command.Items)
        {
            var prod = new Product("Produto Teste", 10m);
            sale.AddSaleItem(prod, item.Quantity, item.UnitPrice);
        }
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(sale));

        var expectedResult = new CreateSaleResult { Id = sale.Id };
        _mapper.Map<CreateSaleResult>(sale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(sale.Id, result.Id);
    }

    [Fact]
    public async Task Handle_ComandoInvalido_LancaValidationException()
    {
        // Arrange: comando inválido (exemplo: SaleNumber zero, supondo que seja inválido)
        var command = _commandFaker.Generate();
        command.SaleNumber = 0;

        // Act & Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_SaleJaExiste_LancaInvalidOperationException()
    {
        // Arrange
        var command = _commandFaker.Generate();
        var existingSale = new Sale(command.SaleNumber, command.SaleDate, new Customer("Cliente Existente"), new Branch("Filial Existente"));
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(existingSale));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains(command.SaleNumber.ToString(), ex.Message);
    }

    [Fact]
    public async Task Handle_ClienteNaoEncontrado_LancaInvalidOperationException()
    {
        // Arrange
        var command = _commandFaker.Generate();
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult<Sale>(null!));

        _customerRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult<Customer>(null!));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains(command.CustomerId.ToString(), ex.Message);
    }

    [Fact]
    public async Task Handle_FilialNaoEncontrada_LancaInvalidOperationException()
    {
        // Arrange
        var command = _commandFaker.Generate();
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult<Sale>(null!));

        var customer = new Customer("Cliente Teste");
        _customerRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(customer));

        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult<Branch>(null!));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains(command.BranchId.ToString(), ex.Message);
    }

    [Fact]
    public async Task Handle_ProdutoNaoEncontrado_LancaInvalidOperationException()
    {
        // Arrange
        var command = _commandFaker.Generate();
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult<Sale>(null!));

        var customer = new Customer("Cliente Teste");
        var branch = new Branch("Filial Teste");
        _customerRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(customer));

        _branchRepository.GetByIdAsync(command.BranchId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(branch));

        // Configurar o repositório de produtos para retornar null para cada item
        foreach (var item in command.Items)
        {
            _productRepository.GetByIdAsync(item.ProductId, Arg.Any<CancellationToken>())!
                .Returns(Task.FromResult<Product>(null!));
        }

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Product with ID", ex.Message);
    }
}