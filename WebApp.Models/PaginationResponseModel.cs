namespace WebApp.Models;

public class PaginationResponseModel<TResponse>
{
    public required IReadOnlyList<TResponse> Items { get; set; }

    public required int TotalItems { get; set; }

    public required int PageNumber { get; set; }

    public required int ItemsPerPage { get; set; }

    public int PagesCount { get; set; }
}
