using Xunit;
using Bogus;
using MediatR;
using AutoMapper;
using NSubstitute;
using System.Reflection;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CancelSaleHandlerTests
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly CancelSaleHandler _handler;
    private readonly IDomainEventStore _eventStore;
    private readonly ISaleRepository _saleRepository;
    private readonly Faker<CancelSaleCommand> _commandFaker;

    public CancelSaleHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediator>();
        _eventStore = Substitute.For<IDomainEventStore>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new CancelSaleHandler(_mapper, _mediator, _eventStore, _saleRepository);
        _commandFaker = new Faker<CancelSaleCommand>()
            .RuleFor(x => x.Id, f => f.Random.Guid());
    }

    [Fact]
    public async Task Handle_ComandoValido_RetornaCancelSaleResult()
    {
        // Arrange:
        var command = _commandFaker.Generate();

        var customer = new Customer("Cliente Teste");
        var branch = new Branch("Filial Teste");
        var sale = new Sale(1, DateTime.UtcNow, customer, branch);
        typeof(Sale).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)?.SetValue(sale, command.Id);

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(sale));

        // Configura o mapper para retornar o resultado esperado
        var expectedResult = new CancelSaleResult { Id = command.Id };
        _mapper.Map<CancelSaleResult>(sale).Returns(expectedResult);

        // Act:
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert:
        Assert.True(sale.IsCanceled);
        Assert.NotNull(result);
        Assert.Equal(command.Id, result.Id);
        await _saleRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ComandoInvalido_LancaValidationException()
    {
        // Arrange: Cria um comando inválido (com Id vazio)
        var command = new CancelSaleCommand { Id = Guid.Empty };

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_SaleNaoEncontrada_LancaKeyNotFoundException()
    {
        // Arrange: Gerar um comando válido, mas configurar o repositório para retornar null (venda não encontrada)
        var command = _commandFaker.Generate();
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult((Sale)null!));

        // Act & Assert: Espera que seja lançada uma KeyNotFoundException contendo o Id do comando
        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains(command.Id.ToString(), ex.Message);
    }
}