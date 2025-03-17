using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.ORM.Extensions;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;


    /// <summary>
    /// Initializes a new instance of UserRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }


    /// <summary>
    /// Creates a new sale in the database
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }


    /// <summary>
    /// Retrieves a sale by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(sale => sale.Branch)
            .Include(sale => sale.Customer)
            .Include(sale => sale.Items).ThenInclude(item => item.Product)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <summary>
    /// Returns filtered and paginated sales.
    /// </summary>
    /// <param name="saleNumber">Filter for sale number</param>
    /// <param name="saleDate">Filter for sale date</param>
    /// <param name="customerId">Filter for customer ID</param>
    /// <param name="branchId">Filter for branch ID</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Number of records per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Object with pagination results and metadata</returns>
    public async Task<PagedResult<Sale>> GetSalesAsync(
        int? saleNumber,
        DateTime? saleDate,
        Guid? customerId,
        Guid? branchId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {

        var query = _context.Sales
            .Include(sale => sale.Branch)
            .Include(sale => sale.Customer)
            .Include(sale => sale.Items).ThenInclude(item => item.Product)
            .AsNoTracking()
            .AsQueryable();

        if (saleNumber.HasValue)
        {
            query = query.Where(s => s.SaleNumber == saleNumber.Value);
        }
        if (saleDate.HasValue)
        {
            query = query.Where(s => s.SaleDate.Date == saleDate.Value.Date);
        }
        if (customerId.HasValue)
        {
            query = query.Where(s => s.Customer.Id == customerId);
        }
        if (branchId.HasValue)
        {
            query = query.Where(s => s.Branch.Id == branchId);
        }

        var paginatedList = await query.OrderBy(s => s.SaleDate)
            .ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);


        return new PagedResult<Sale>
        {
            Items = paginatedList.Items,
            TotalCount = paginatedList.TotalCount,
            TotalPages = paginatedList.TotalPages,
            CurrentPage = paginatedList.CurrentPage
        };
    }

    /// <summary>
    /// Retrieves a sale by their sale number
    /// </summary>
    /// <param name="saleNumber">The saleNumber to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<Sale?> GetBySaleNumberAsync(int saleNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.FirstOrDefaultAsync(o => o.SaleNumber == saleNumber, cancellationToken);
    }

    /// <summary>
    /// Updates a sale in the database
    /// </summary>
    /// <param name="sale">sale for update</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Task return</returns>
    public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        var existingSale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == sale.Id, cancellationToken);

        if (existingSale == null)
        {
            throw new Exception("Sale not found");
        }

        _context.Entry(existingSale).CurrentValues.SetValues(sale);

        foreach (var item in sale.Items)
        {
            var existingItem = existingSale.Items.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(item);
            }
            else
            {
                existingSale.AddSaleItem(item.Product, item.Quantity, item.UnitPrice);
            }
        }

        foreach (var existingItem in existingSale.Items.ToList())
        {
            if (sale.Items.Any(i => i.Id == existingItem.Id) is false)
            {
                existingSale.RemoveItem(existingItem);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously saves all changes made in the context to the persistent storage.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the save operation.</param>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}