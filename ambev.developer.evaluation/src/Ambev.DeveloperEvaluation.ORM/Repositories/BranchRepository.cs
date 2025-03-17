using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class BranchRepository(DefaultContext context) : IBranchRepository
{
    /// <summary>
    /// Retrieves a branch by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the branch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The branch customer</returns>
    public async Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Branchs.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}