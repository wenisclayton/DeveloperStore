using Ambev.DeveloperEvaluation.Common.Extensions;
using Ambev.DeveloperEvaluation.Domain.Common;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

/// <summary>
/// Represents a customer who can make purchases.
/// </summary>
public class Customer : BaseEntity
{
    /// <summary>
    /// Gets the name of the customer.
    /// </summary>
    public string Name { get; private set; }

    [ExcludeFromCodeCoverage]
    private Customer() { } // Required for serialization and ORM

    /// <summary>
    /// Initializes a new instance of the <see cref="Customer"/> class.
    /// </summary>
    /// <param name="name">name</param>
    /// <exception cref="ArgumentException">argument exception</exception>
    /// <exception cref="ArgumentNullException">argument null exception</exception>
    public Customer(string name)
    {
        if (name.IsNullOrWhiteSpace())
            throw new ArgumentException("Customer name cannot be empty.", nameof(name));

        Id = Guid.NewGuid();

        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}