using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Common.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

public class Branch : BaseEntity
{
    /// <summary>
    /// Gets the name of the branch.
    /// </summary>
    public string Name { get; private set; }

    [ExcludeFromCodeCoverage]
    private Branch() { } // Required for serialization and ORM

    /// <summary>
    /// Initializes a new instance of the <see cref="Branch"/> class.
    /// </summary>
    /// <param name="name">name</param>
    /// <exception cref="ArgumentException">argument exception</exception>
    /// <exception cref="ArgumentNullException">argument null exception</exception>
    public Branch(string name)
    {
        if (name.IsNullOrWhiteSpace())
            throw new ArgumentException("Branch name cannot be empty.", nameof(name));

        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}