namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Encapsulates the sales query results with pagination information.
/// </summary>
public class GetSalesResultPaged
{
    /// <summary>
    /// List of sales returned.
    /// </summary>
    public IEnumerable<GetSalesResult> Items { get; set; }

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