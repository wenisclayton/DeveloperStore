
using Bogus;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Sale-related entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Sale entities.
    /// The generated sale will have a valid sale number, sale date, customer, and branch.
    /// </summary>
    private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
        .CustomInstantiator(f => new Sale(
            f.Random.Number(1, 1000),
            f.Date.Recent(10),
            GenerateValidCustomer(),
            GenerateValidBranch()
        ));

    /// <summary>
    /// Configures the Faker to generate valid Customer entities.
    /// The generated customer will have a valid name.
    /// Note: Adjust the constructor parameters according to the actual implementation of Customer.
    /// </summary>
    private static readonly Faker<Customer> CustomerFaker = new Faker<Customer>()
        .CustomInstantiator(f => new Customer(f.Person.FullName));

    /// <summary>
    /// Configures the Faker to generate valid Branch entities.
    /// The generated branch will have a valid name and code.
    /// Note: Adjust the constructor parameters according to the actual implementation of Branch.
    /// </summary>
    private static readonly Faker<Branch> BranchFaker = new Faker<Branch>()
        .CustomInstantiator(f => new Branch(f.Company.CompanyName()));

    /// <summary>
    /// Configures the Faker to generate valid Product entities.
    /// The generated product will have a valid product name and price.
    /// Note: Adjust the constructor parameters according to the actual implementation of Product.
    /// </summary>
    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .CustomInstantiator(f => new Product(f.Commerce.ProductName(), f.Random.Decimal(1, 100)));

    /// <summary>
    /// Configures the Faker to generate valid SaleItem entities.
    /// The generated SaleItem will have a valid product, quantity, unit price and discount.
    /// </summary>
    private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
        .CustomInstantiator(f =>
        {
            var product = GenerateValidProduct();
            var quantity = f.Random.Int(1, 10);
            var unitPrice = f.Random.Decimal(1, 100);
            // Garante que o desconto seja entre 0 e (unitPrice * quantity)
            var discount = f.Random.Decimal(0, unitPrice * quantity);
            return new SaleItem(product, quantity, unitPrice, discount);
        });

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// </summary>
    /// <returns>A valid Sale entity.</returns>
    public static Sale GenerateValidSale()
    {
        return SaleFaker.Generate();
    }

    /// <summary>
    /// Generates a valid Customer entity with randomized data.
    /// </summary>
    /// <returns>A valid Customer entity.</returns>
    public static Customer GenerateValidCustomer()
    {
        return CustomerFaker.Generate();
    }

    /// <summary>
    /// Generates a valid Branch entity with randomized data.
    /// </summary>
    /// <returns>A valid Branch entity.</returns>
    public static Branch GenerateValidBranch()
    {
        return BranchFaker.Generate();
    }

    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// </summary>
    /// <returns>A valid Product entity.</returns>
    public static Product GenerateValidProduct()
    {
        return ProductFaker.Generate();
    }

    /// <summary>
    /// Generates a valid SaleItem entity with randomized data.
    /// </summary>
    /// <returns>A valid SaleItem entity.</returns>
    public static SaleItem GenerateValidSaleItem()
    {
        return SaleItemFaker.Generate();
    }
}