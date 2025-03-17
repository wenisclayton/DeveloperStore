using Xunit;
using Bogus;
using NSubstitute;
using AutoMapper;
using System.Reflection;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;


namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class GetSaleHandlerTests
{
    private readonly IMapper _mapper;
    private readonly ISaleRepository _saleRepository;
    private readonly GetSaleHandler _handler;
    private readonly Faker<GetSaleCommand> _commandFaker;

    public GetSaleHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new GetSaleHandler(_saleRepository, _mapper);

        _commandFaker = new Faker<GetSaleCommand>()
            .RuleFor(x => x.Id, f => f.Random.Guid());
    }

    [Fact]
    public async Task Handle_ComandoValido_RetornaGetSaleResult()
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

        var expectedResult = new GetSaleResult { Id = command.Id };
        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Id, result.Id);
    }

    [Fact]
    public async Task Handle_ComandoInvalido_LancaValidationException()
    {
        // Arrange: comando inválido com Id vazio
        var command = new GetSaleCommand { Id = Guid.Empty };

        // Act & Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_SaleNaoEncontrada_LancaKeyNotFoundException()
    {
        // Arrange
        var command = _commandFaker.Generate();
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult<Sale>(null!));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains(command.Id.ToString(), ex.Message);
    }
}