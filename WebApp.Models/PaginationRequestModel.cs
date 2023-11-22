namespace WebApp.Models;

public class PaginationRequestModel
{
    private readonly int DefaultItemsPerPage = 10;
    private readonly int DefaultPageNumber = 1;

    private int? itemsPerPage;
    private int? pageNumber;

    public int? ItemsPerPage
    {
        get
        {
            if (itemsPerPage == null || itemsPerPage <= 0)
                itemsPerPage = DefaultItemsPerPage;

            return itemsPerPage;
        }
        set => itemsPerPage = value;
    }

    public int? PageNumber
    {
        get
        {
            if (pageNumber is null || pageNumber <= 0)
                pageNumber = DefaultPageNumber;

            return pageNumber;
        }
        set => pageNumber = value;
    }

    public int SkipCount => (PageNumber!.Value - 1) * ItemsPerPage!.Value;
}
