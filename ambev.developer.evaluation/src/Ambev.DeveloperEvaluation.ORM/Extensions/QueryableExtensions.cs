using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> source,
        int currentPage,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var totalCount = await source.CountAsync(cancellationToken);
        var items = await source.Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, totalCount, currentPage, pageSize);
    }
}