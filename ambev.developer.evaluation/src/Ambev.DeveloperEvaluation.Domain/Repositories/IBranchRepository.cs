using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IBranchRepository
{
    /// <summary>
    /// Retrieves a branch by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the branch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The branch if found, null otherwise</returns>
    Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}