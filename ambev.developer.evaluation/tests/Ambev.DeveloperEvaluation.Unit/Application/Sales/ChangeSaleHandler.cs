using Xunit;
using Bogus;
using MediatR;
using AutoMapper;
using NSubstitute;
using System.Reflection;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.ChangeSale;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;
public class ChangeSaleHandlerTests
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ChangeSaleHandler _handler;
    private readonly IDomainEventStore _eventStore;
    private readonly ISaleRepository _saleRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly Faker<ChangeSaleCommand> _commandFaker;
    private readonly ICustomerRepository _customerRepository;

    public ChangeSaleHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediator>();
        _eventStore = Substitute.For<IDomainEventStore>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();

        _handler = new ChangeSaleHandler(_mapper, _mediator, _eventStore, _saleRepository, _branchRepository, _productRepository, _customerRepository);

        _commandFaker = new Faker<ChangeSaleCommand>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.SaleNumber, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.SaleDate, f => f.Date.Recent())
            .RuleFor(x => x.CustomerId, f => f.Random.Guid())
            .RuleFor(x => x.BranchId, f => f.Random.Guid())
            .RuleFor(x => x.Items, f => new List<ChangeSaleItemCommand> {
                    new ChangeSaleItemCommand {
                        ProductId = f.Random.Guid(),
                        Quantity = f.Random.Int(1, 5),
                        UnitPrice = f.Random.Decimal(1, 100)
                    }
            });
    }

    [Fact]
    public async Task Handle_ComandoValido_RetornaChangeSaleResult()
    {
        // Arrange
        var command = _commandFaker.Generate();

        
        var customer = new Customer("Cliente Teste");
        var branch = new Branch("Filial Teste");
        var sale = new Sale(1, DateTime.UtcNow, customer, branch);


        typeof(Sale)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(sale, command.Id);

        
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(sale));

        _customerRepository.GetByIdAsync(command.CustomerId!.Value, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(customer));

        _branchRepository.GetByIdAsync(command.BranchId!.Value, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(branch));

        // Para cada item, configurar o repositório de produtos
        foreach (var item in command.Items!)
        {
            var product = new Product("Produto Teste", 10m);
            typeof(Product)
                .GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!
                .SetValue(product, item.ProductId);

            _productRepository.GetByIdAsync(item.ProductId!.Value, Arg.Any<CancellationToken>())!
                .Returns(Task.FromResult(product));
        }

        var expectedResult = new ChangeSaleResult { Id = command.Id };
        _mapper.Map<ChangeSaleResult>(sale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Id, result.Id);
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_SaleNaoEncontrada_LancaInvalidOperationException()
    {
        // Arrange
        var command = _commandFaker.Generate();
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult<Sale>(null!));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains(command.Id.ToString(), ex.Message);
    }

    [Fact]
    public async Task Handle_ClienteNaoEncontrado_LancaInvalidOperationException()
    {
        // Arrange
        var command = _commandFaker.Generate();
        var sale = new Sale(1, DateTime.UtcNow, new Customer("Cliente Teste"), new Branch("Filial Teste"));
        typeof(Sale)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(sale, command.Id);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(sale));

        _customerRepository.GetByIdAsync(command.CustomerId!.Value, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult<Customer>(null!));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains(command.CustomerId.Value.ToString(), ex.Message);
    }

    [Fact]
    public async Task Handle_FilialNaoEncontrada_LancaInvalidOperationException()
    {
        // Arrange
        var command = _commandFaker.Generate();
        var customer = new Customer("Cliente Teste");
        var sale = new Sale(1, DateTime.UtcNow, customer, new Branch("Filial Teste"));
        typeof(Sale)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(sale, command.Id);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(sale));

        _customerRepository.GetByIdAsync(command.CustomerId!.Value, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(customer));

        _branchRepository.GetByIdAsync(command.BranchId!.Value, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult<Branch>(null!));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains(command.BranchId.Value.ToString(), ex.Message);
    }

    [Fact]
    public async Task Handle_ProdutoNaoEncontrado_LancaInvalidOperationException()
    {
        // Arrange
        var command = _commandFaker.Generate();
        var customer = new Customer("Cliente Teste");
        var branch = new Branch("Filial Teste");
        var sale = new Sale(1, DateTime.UtcNow, customer, branch);
        typeof(Sale)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(sale, command.Id);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(sale));

        _customerRepository.GetByIdAsync(command.CustomerId!.Value, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(customer));

        _branchRepository.GetByIdAsync(command.BranchId!.Value, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(branch));

        // Configurar repositório de produtos para retornar null para cada item
        foreach (var item in command.Items!)
        {
            _productRepository.GetByIdAsync(item.ProductId!.Value, Arg.Any<CancellationToken>())!
                .Returns(Task.FromResult<Product>(null!));
        }

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Product with id ", ex.Message);
    }
}
