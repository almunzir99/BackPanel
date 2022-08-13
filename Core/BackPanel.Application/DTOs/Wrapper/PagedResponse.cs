namespace BackPanel.Application.DTOs.Wrapper;

public class PagedResponse<T> : Response<T>{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public Uri FirstPage { get; set; }
    public Uri LastPage { get; set; }
    public Uri? NextPage { get; set; }
    public Uri? PreviousPage { get; set; }
    public PagedResponse(T data,
        int pageSize,
        int pageIndex,
        int totalPages,
        int totalRecords,
        Uri firstPage,
        Uri lastPage,
        Uri? nextPage,
        Uri? previousPage) : base(data)
    {
        PageSize = pageSize;
        PageIndex = pageIndex;
        TotalPages = totalPages;
        TotalRecords = totalRecords;
        FirstPage = firstPage;
        LastPage = lastPage;
        NextPage = nextPage;
        PreviousPage = previousPage;
    }

}