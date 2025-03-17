namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Result returned after creating a sale.
/// </summary>
public class CreateSaleResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the newly created user.
    /// </summary>
    /// <value>A GUID that uniquely identifies the created user in the system.</value>
    public Guid Id { get; set; }
}