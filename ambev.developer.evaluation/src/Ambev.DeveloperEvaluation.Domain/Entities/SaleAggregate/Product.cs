using System.Diagnostics.CodeAnalysis;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Common.Extensions;

namespace Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

/// <summary>
/// Represents a product that can be sold.
/// </summary>
public class Product: BaseEntity
{
    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; private set; }


    [ExcludeFromCodeCoverage]
    private Product() { } // Required for serialization and ORM

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="unitPrice">unit price</param>
    /// <exception cref="ArgumentException">argument exception</exception>
    /// <exception cref="ArgumentOutOfRangeException">argument out of range exception</exception>
    public Product(string name, decimal unitPrice)
    {
        if (name.IsNullOrWhiteSpace())
            throw new ArgumentException("The product name cannot be empty.", nameof(name));

        if (unitPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");

        Id = Guid.NewGuid();
        Name = name;
        UnitPrice = unitPrice;
    }
}