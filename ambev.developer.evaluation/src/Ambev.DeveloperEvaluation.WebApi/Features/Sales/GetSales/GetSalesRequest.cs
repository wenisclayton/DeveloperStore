namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Represents the request to retrieve a sale
/// </summary>
public class GetSalesRequest
{
    /// <summary>
    /// Filter for  sale number. If provided, will return only sale with this sale number.
    /// </summary>
    public int? SaleNumber { get; set; }

    /// <summary>
    /// Filter for sale date. If provided, will return only sales with this date.
    /// </summary>
    public DateTime? SaleDate { get; set; }

    /// <summary>
    /// Filter for customer ID. If provided, will return only sales related to this customer.
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Filter for branch ID. If provided, will return only sales related to this branch.
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Page number for pagination.
    /// </summary>
    public int PageNumber { get; set; } = 1; /// <summary>
    /// Number of records per page.
    /// </summary>
    public int PageSize { get; set; } = 10;
}