namespace Ambev.DeveloperEvaluation.Common.Pagination;

/// <summary>
/// Generic class to encapsulate paged results.
/// </summary>
public class PagedResult<T>
{
    /// <summary>
    /// Items returned in the query.
    /// </summary>
    public IEnumerable<T> Items { get; set; }

    /// <summary>
    /// Total records found.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Total pages.
    /// </summary>
    public int TotalPages { get; set; }
}