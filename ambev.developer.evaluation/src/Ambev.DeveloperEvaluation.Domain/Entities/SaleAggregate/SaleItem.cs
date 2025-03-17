using Ambev.DeveloperEvaluation.Domain.Common;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

/// <summary>
/// Represents a sale item in a sale.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets the product of the sale item.
    /// </summary>
    public Product Product { get; private set; }

    /// <summary>
    /// Gets the unit price of the sale item.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the quantity of the sale item.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the discount of the sale item.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the sale item was canceled.
    /// </summary>
    public bool IsCanceled { get; private set; }

    /// <summary>
    /// Gets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time of the last update to the user's information.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the total value of the sale item.
    /// </summary>
    public decimal TotalValue => (UnitPrice * Quantity) - Discount;

    [ExcludeFromCodeCoverage]
    private SaleItem() { }


    /// <summary>
    /// Initializes a new instance of the <see cref="SaleItem"/> class.
    /// </summary>
    /// <param name="product">Product of the sale item.</param>
    /// <param name="quantity">Quantity of the sale item.</param>
    /// <param name="unitPrice">UnitePrice of the sale item.</param>
    /// <param name="discount">Discount of the sale item.</param>
    public SaleItem(Product product, int quantity, decimal unitPrice, decimal discount)
    {
        ValidateProduct(product);
        ValidateQuantity(quantity);
        ValidateUnitPrice(unitPrice);
        ValidateDiscount(discount, unitPrice, quantity);

        Id = Guid.NewGuid();
        Product = product;
        IsCanceled = false;
        Quantity = quantity;
        Discount = discount;
        UnitPrice = unitPrice;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Change the sale item.
    /// </summary>
    /// <param name="quantity">quantity of the sale item.</param>
    /// <param name="unitPrice">unit price of the sale item.</param>
    /// <param name="discount">discount of the sale item.</param>
    public void Change(int quantity, decimal unitPrice, decimal discount)
    {
        ValidateQuantity(quantity);
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
    }

    /// <summary>
    /// Cancel the sale item.
    /// </summary>
    public void Cancel()
    {
        IsCanceled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    // Métodos de validação
    private void ValidateProduct(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");
    }

    private void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
    }

    private void ValidateUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");
    }

    private void ValidateDiscount(decimal discount, decimal unitPrice, int quantity)
    {
        if (discount < 0)
            throw new ArgumentOutOfRangeException(nameof(discount), "Discount cannot be negative.");

        decimal totalPrice = unitPrice * quantity;
        if (discount > totalPrice)
            throw new ArgumentException("Discount cannot be greater than the total item price.", nameof(discount));
    }
}