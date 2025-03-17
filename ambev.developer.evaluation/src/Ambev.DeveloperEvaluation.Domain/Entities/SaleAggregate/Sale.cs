using Ambev.DeveloperEvaluation.Domain.Common;
using System.Diagnostics.CodeAnalysis;


namespace Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

/// <summary>
/// Represents a sale made by a customer in a branch.
/// </summary>
public class Sale : BaseEntity
{
    private readonly List<SaleItem> _items;
    
    /// <summary>
    /// Gets the sale number.
    /// </summary>
    public int SaleNumber { get; private set; }

    /// <summary>
    /// Gets the sale date.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Gets the customer who made the purchase.
    /// </summary>
    public Customer Customer { get; private set; }

    /// <summary>
    /// Gets the branch where the sale was made.
    /// </summary>
    public Branch Branch { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the sale was canceled.
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
    /// Gets the sale items.
    /// </summary>
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Gets the total value of the sale.
    /// </summary>
    public decimal TotalValue => Items.Sum(i => i.TotalValue);

    [ExcludeFromCodeCoverage]
    private Sale() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sale"/> class.
    /// </summary>
    /// <param name="saleNumber">sale number</param>
    /// <param name="saleDate">sale date</param>
    /// <param name="customer">customer</param>
    /// <param name="branch">branch</param>
    public Sale(int saleNumber, DateTime saleDate, Customer customer, Branch branch)
    {
        ValidateSaleNumber(saleNumber);
        ValidateCustomer(customer);
        ValidateBranch(branch);

        Id = Guid.NewGuid();
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        Customer = customer;
        Branch = branch;
        IsCanceled = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        
        _items = new List<SaleItem>();
    }

    /// <summary>
    /// Add a new item to the sale.
    /// </summary>
    /// <param name="product">product</param>
    /// <param name="quantity">quantity</param>
    /// <param name="unitPrice">unit price</param>
    public void AddSaleItem(Product product, int quantity, decimal unitPrice)
    {
        ValidateSaleNotCanceled();
        ValidateQuantity(quantity);
        var discountCalculated = CalculateDiscount(quantity, unitPrice);
        var item = new SaleItem(product, quantity, unitPrice, discountCalculated);
        _items.Add(item);
    }

    /// <summary>
    /// Remove an item from the sale.
    /// </summary>
    /// <param name="item">item</param>
    public void RemoveItem(SaleItem item)
    {
        _items.Remove(item);
    }

    /// <summary>
    /// Cancel the sale.
    /// </summary>
    public void Cancel()
    {
        IsCanceled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancel a sale item.
    /// </summary>
    /// <param name="saleItemId">sale item Id</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>sale item</returns>
    /// <exception cref="KeyNotFoundException">key not found</exception>
    public SaleItem CancelSaleItem(Guid saleItemId, CancellationToken cancellationToken)
    {
        if (Items.Count <= 0) 
            throw new KeyNotFoundException($"User with ID {saleItemId} not found");

        var saleItemFound = Items.FirstOrDefault(item => item.Id == saleItemId);
        if (saleItemFound == null)
            throw new KeyNotFoundException($"User with ID {saleItemId} not found");

        saleItemFound.Cancel();
        return saleItemFound;
    }


    /// <summary>
    /// Change the sale consistently.
    /// </summary>
    /// <param name="saleNumber">sale number</param>
    /// <param name="saleDate">sale date</param>
    /// <param name="customer">customer</param>
    /// <param name="branch">branch</param>
    public void Change(int? saleNumber, DateTime? saleDate, Customer? customer, Branch? branch)
    {
        var change = saleNumber.HasValue || customer is not null || branch is not null || saleDate.HasValue;

        if (saleNumber.HasValue)
        {
            ValidateSaleNumber(saleNumber.Value);
            SaleNumber = saleNumber.Value;
        }
        if (customer is not null)
        {
            ValidateCustomer(customer);
            Customer = customer;
        }
        if (branch is not null)
        {
            ValidateBranch(branch);
            Branch = branch;
        }
        if (saleDate.HasValue)
        {
            SaleDate = saleDate.Value;
        }
        if(change)
                UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Change the sale items.
    /// </summary>
    /// <param name="newItems">new items for change</param>
    /// <exception cref="InvalidOperationException">invalid operation</exception>
    public void ChangeSaleItems(IEnumerable<(Product product, int? quantity, decimal? unitPrice)> newItems)
    {
        if (IsCanceled)
            throw new InvalidOperationException("Cannot update items of a cancelled sale.");

        var itemsToRemove = _items
            .Where(existingItem => newItems.Any(newItem => newItem.product.Id == existingItem.Product.Id) is false)
            .ToList();

        foreach (var item in itemsToRemove)
        {
            _items.Remove(item);
        }

        
        foreach (var newItem in newItems)
        {
            var existingItem = _items.FirstOrDefault(item => item.Product.Id == newItem.product.Id);
            var quantity = newItem.quantity ?? existingItem!.Quantity;
            var unitPrice = newItem.unitPrice ?? existingItem!.UnitPrice;

            if (existingItem is not null)
            {
                var discountCalculated = CalculateDiscount(quantity, unitPrice);
                existingItem.Change(quantity, unitPrice, discountCalculated);
            }
            else
            {
                AddSaleItem(newItem.product, quantity, unitPrice);
            }
        }

        UpdatedAt = DateTime.UtcNow;
    }


    private static readonly Dictionary<Func<int, bool>, decimal> DiscountRules = new()
    {
        { quantity => quantity is >= 4 and < 10, 0.10m },
        { quantity => quantity is >= 10 and <= 20, 0.20m }
    };

    private decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        var discountPercentage = DiscountRules.FirstOrDefault(rule => rule.Key(quantity)).Value;
        return unitPrice * quantity * discountPercentage;
    }

    private void ValidateSaleNumber(int saleNumber)
    {
        if (saleNumber < 0)
            throw new ArgumentException("Sale number cannot be '0' .", nameof(saleNumber));
    }

    private void ValidateCustomer(Customer customer)
    {
        if (customer == null)
            throw new ArgumentNullException(nameof(customer));
    }

    private void ValidateBranch(Branch branch)
    {
        if (branch == null)
            throw new ArgumentNullException(nameof(branch));
    }

    private void ValidateSaleNotCanceled()
    {
        if (IsCanceled)
            throw new InvalidOperationException("Cannot add items to a canceled sale.");
    }

    private void ValidateQuantity(int quantity)
    {
        if (quantity < 1)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least 1.");

        if (quantity > 20)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Cannot purchase more than 20 units of the same product.");
    }
}