using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using global::Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.SaleAggregate;

/// <summary>
/// Contém testes de unidade para a entidade Sale.
/// Os testes cobrem criação, adição de itens, cálculo de descontos, cancelamento e atualizações.
/// </summary>
public class SaleTests
{
    [Fact(DisplayName = "Sale must be created with valid data")]
    public void Given_ValidSaleData_When_Created_Then_SaleShouldHaveExpectedProperties()
    {
        // Arrange
        var saleNumber = 1;
        var saleDate = DateTime.UtcNow;
        var customer = SaleTestData.GenerateValidCustomer();
        var branch = SaleTestData.GenerateValidBranch();

        // Act
        var sale = new Sale(saleNumber, saleDate, customer, branch);

        // Assert
        Assert.Equal(saleNumber, sale.SaleNumber);
        Assert.Equal(saleDate, sale.SaleDate);
        Assert.Equal(customer, sale.Customer);
        Assert.Equal(branch, sale.Branch);
        Assert.False(sale.IsCanceled);
        Assert.NotEqual(default(DateTime), sale.CreatedAt);
        Assert.NotNull(sale.UpdatedAt);
        Assert.Empty(sale.Items);
    }

    [Fact(DisplayName = "Adding item should increase total value with discount applied")]
    public void Given_ValidItemData_When_AddingItem_Then_TotalValueShouldBeCalculatedCorrectly()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = SaleTestData.GenerateValidProduct();
        int quantity = 4; // 4 itens se enquadram na regra de desconto (>= 4 e < 10)
        decimal unitPrice = 10m;

        // Desconto calculado: 10% de 10 * 4 = 4
        decimal expectedDiscount = unitPrice * quantity * 0.10m;
        decimal expectedTotalValue = (unitPrice * quantity) - expectedDiscount;

        // Act
        // O parâmetro 'discount' passado não é utilizado, pois o método calcula o desconto internamente.
        sale.AddSaleItem(product, quantity, unitPrice);

        // Assert
        Assert.Single(sale.Items);
        Assert.Equal(expectedTotalValue, sale.TotalValue);
    }

    [Fact(DisplayName = "Adding item to cancelled sale should throw exception")]
    public void Given_CanceledSale_When_AddingItem_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Cancel();
        var product = SaleTestData.GenerateValidProduct();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => sale.AddSaleItem(product, 5, 10m));
    }

    [Fact(DisplayName = "Updating sale should change sale properties")]
    public void Given_ValidSale_When_Updated_Then_SalePropertiesShouldBeChanged()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        int newSaleNumber = sale.SaleNumber + 1;
        DateTime newSaleDate = sale.SaleDate.AddDays(1);
        var newCustomer = SaleTestData.GenerateValidCustomer(); // Instância diferente
        var newBranch = SaleTestData.GenerateValidBranch();     // Instância diferente

        // Act
        sale.Change(newSaleNumber, newSaleDate, newCustomer, newBranch);

        // Assert
        Assert.Equal(newSaleNumber, sale.SaleNumber);
        Assert.Equal(newSaleDate, sale.SaleDate);
        Assert.Equal(newCustomer, sale.Customer);
        Assert.Equal(newBranch, sale.Branch);
        Assert.True(sale.UpdatedAt >= sale.CreatedAt);
    }

    [Fact(DisplayName = "Updating sale items should replace existing items")]
    public void Given_ValidSale_When_UpdatingSaleItems_Then_ItemsShouldBeReplaced()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        // Adiciona um item inicialmente
        var product1 = SaleTestData.GenerateValidProduct();
        sale.AddSaleItem(product1, 4, 10m);

        // Define novos itens para atualizar a venda
        var product2 = SaleTestData.GenerateValidProduct();
        var newItems = new (Product product, int? quantity, decimal? unitPrice)[]
        {
            // Para 5 unidades: desconto de 10% (>=4 e <10)
            (product: product2, quantity: 5, unitPrice: 20m),
            // Para 10 unidades: desconto de 20% (>=10 e <=20)
            (product: product1, quantity: 10, unitPrice: 15m)
        };

        // Act
        sale.ChangeSaleItems(newItems);

        // Assert
        Assert.Equal(newItems.Length, sale.Items.Count);
        decimal expectedTotalValue =
            ((20m * 5) - (20m * 5 * 0.10m)) +
            ((15m * 10) - (15m * 10 * 0.20m));
        Assert.Equal(expectedTotalValue, sale.TotalValue);
    }

    [Fact(DisplayName = "Updating items in cancelled sale should throw exception")]
    public void Given_CanceledSale_When_UpdatingSaleItems_Then_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Cancel();
        var product = SaleTestData.GenerateValidProduct();
        var newItems = new (Product product, int? quantity, decimal? unitPrice)[]
        {
            (product: product, quantity: 5, unitPrice: 10m)
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => sale.ChangeSaleItems(newItems));
    }

    [Fact(DisplayName = "Cancelling a non-existent sale item should throw exception")]
    public void Given_NonExistentSaleItem_When_CancellingSaleItem_Then_ShouldThrowKeyNotFoundException()
    {
        // Arrange: gera uma venda sem adicionar nenhum item
        var sale = SaleTestData.GenerateValidSale();
        var nonExistentItemId = Guid.NewGuid();

        // Act & Assert: deve lançar KeyNotFoundException ao tentar cancelar um item inexistente
        var exception = Assert.Throws<KeyNotFoundException>(() => sale.CancelSaleItem(nonExistentItemId, CancellationToken.None));
        Assert.Contains(nonExistentItemId.ToString(), exception.Message);
    }

    [Fact(DisplayName = "Cancelling an existing sale item should mark it as cancelled")]
    public void Given_ExistingSaleItem_When_CancellingSaleItem_Then_ItemIsMarkedAsCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var saleItem = SaleTestData.GenerateValidSaleItem();
        sale.AddSaleItem(saleItem.Product, saleItem.Quantity, saleItem.UnitPrice);
        var addedSaleItem = sale.Items.First();

        // Act
        var cancelledItem = sale.CancelSaleItem(addedSaleItem.Id, CancellationToken.None);

        // Assert
        Assert.True(cancelledItem.IsCanceled, "The sale item should be marked as canceled.");
        Assert.True(sale.UpdatedAt >= sale.CreatedAt, "The sale's UpdatedAt should be updated after cancellation.");
    }

    [Fact(DisplayName = "Creating sale with invalid number should throw exception")]
    public void Given_InvalidSaleNumber_When_Created_Then_ShouldThrowArgumentException()
    {
        // Arrange
        int invalidSaleNumber = -1;
        var saleDate = DateTime.UtcNow;
        var customer = SaleTestData.GenerateValidCustomer();
        var branch = SaleTestData.GenerateValidBranch();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Sale(invalidSaleNumber, saleDate, customer, branch));
    }

    [Fact(DisplayName = "Creating sale with null customer should throw exception")]
    public void Given_NullCustomer_When_Created_Then_ShouldThrowArgumentNullException()
    {
        // Arrange
        int saleNumber = 1;
        var saleDate = DateTime.UtcNow;
        Customer customer = null;
        var branch = SaleTestData.GenerateValidBranch();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Sale(saleNumber, saleDate, customer, branch));
    }

    [Fact(DisplayName = "Creating sale with null branch should throw exception")]
    public void Given_NullBranch_When_Created_Then_ShouldThrowArgumentNullException()
    {
        // Arrange
        int saleNumber = 1;
        var saleDate = DateTime.UtcNow;
        var customer = SaleTestData.GenerateValidCustomer();
        Branch branch = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Sale(saleNumber, saleDate, customer, branch));
    }

    [Fact(DisplayName = "Adding item with invalid quantity should throw exception")]
    public void Given_InvalidQuantity_When_AddingItem_Then_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var product = SaleTestData.GenerateValidProduct();

        // Act & Assert para quantidade menor que 1
        Assert.Throws<ArgumentOutOfRangeException>(() => sale.AddSaleItem(product, 0, 10m));

        // Act & Assert para quantidade maior que 20
        Assert.Throws<ArgumentOutOfRangeException>(() => sale.AddSaleItem(product, 21, 10m));
    }
}