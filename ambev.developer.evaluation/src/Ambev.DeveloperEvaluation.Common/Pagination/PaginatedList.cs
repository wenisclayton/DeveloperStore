public class PaginatedList<T> : List<T>
{
    /// <summary>
    /// Lista de itens da página atual.
    /// </summary>
    public List<T> Items { get; private set; }

    /// <summary>
    /// Página atual.
    /// </summary>
    public int CurrentPage { get; private set; }

    /// <summary>
    /// Total de páginas.
    /// </summary>
    public int TotalPages { get; private set; }

    /// <summary>
    /// Total de registros encontrados.
    /// </summary>
    public int TotalCount { get; private set; }

    /// <summary>
    /// Quantidade de registros por página.
    /// </summary>
    public int PageSize { get; private set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PaginatedList(List<T> items, int totalCount, int currentPage, int pageSize)
        : base(items)
    {
        Items = items;
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}