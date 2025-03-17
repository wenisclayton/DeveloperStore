using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    /// <summary>
    /// Creates a new sale in the repository
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by their sale number
    /// </summary>
    /// <param name="saleNumber">The saleNumber to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetBySaleNumberAsync(int saleNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update a sale in the repository
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale updated</returns>
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously saves all changes made in the context to the persistent storage.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the save operation.</param>
    /// <returns>A task representing the asynchronous save operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns filtered and paginated sales.
    /// </summary>
    /// <param name="saleNumber">Filter for sale date</param>
    /// <param name="saleDate">Filter for sale date</param>
    /// <param name="customerId">Filter for customer ID</param>
    /// <param name="branchId">Filter for branch ID</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Number of records per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Object with pagination results and metadata</returns>
    Task<PagedResult<Sale>> GetSalesAsync(
int? saleNumber,
                    DateTime? saleDate,
                    Guid? customerId,
                    Guid? branchId,
                    int pageNumber,
                    int pageSize,
                    CancellationToken cancellationToken);

}