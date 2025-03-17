using Xunit;
using Bogus;
using MediatR;
using AutoMapper;
using NSubstitute;
using System.Reflection;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;


namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CancelSaleItemHandlerTests
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IDomainEventStore _eventStore;
    private readonly ISaleRepository _saleRepository;
    private readonly CancelSaleItemHandler _handler;
    private readonly Faker<CancelSaleItemCommand> _commandFaker;

    public CancelSaleItemHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediator>();
        _eventStore = Substitute.For<IDomainEventStore>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new CancelSaleItemHandler(_mapper, _mediator, _eventStore, _saleRepository);

        _commandFaker = new Faker<CancelSaleItemCommand>()
            .RuleFor(x => x.SaleId, f => f.Random.Guid())
            .RuleFor(x => x.SaleItemId, f => f.Random.Guid());
    }

    [Fact]
    public async Task Handle_ComandoValido_RetornaCancelSaleItemResult()
    {
        // Arrange
        var command = _commandFaker.Generate();

        var customer = new Customer("Cliente Teste");
        var branch = new Branch("Filial Teste");
        var sale = new Sale(1, DateTime.UtcNow, customer, branch);
        var product = new Product("Produto Teste", 10m);
        sale.AddSaleItem(product, 2, 10m);

        // Obter o SaleItem criado e ajustar seu Id para coincidir com o do comando
        var saleItem = sale.Items.First();

        typeof(SaleItem)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(saleItem, command.SaleItemId);

        // Ajustar o Id da sale para coincidir com o comando
        typeof(Sale)
            .GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(sale, command.SaleId);

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(sale));

        // Configurar o mapper para mapear o saleItem para o resultado esperado
        var expectedResult = new CancelSaleItemResult { Id = command.SaleItemId };
        _mapper.Map<CancelSaleItemResult>(saleItem).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.SaleItemId, result.Id);
        Assert.True(saleItem.IsCanceled);
        await _saleRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ComandoInvalido_LancaValidationException()
    {
        // Arrange: comando inválido com SaleId e SaleItemId vazios
        var command = new CancelSaleItemCommand { SaleId = Guid.Empty, SaleItemId = Guid.Empty };

        // Act & Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_SaleNaoEncontrada_LancaKeyNotFoundException()
    {
        // Arrange: comando válido, mas repositório retorna null
        var command = _commandFaker.Generate();
        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult<Sale>(null!));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains(command.SaleId.ToString(), ex.Message);
    }
}