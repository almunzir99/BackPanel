namespace BackPanel.Application.DTOs.Filters;

public class PaginationFilter
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }

    public PaginationFilter()
    {
        this.PageIndex = 1;
        this.PageSize = 10;
    }

    public PaginationFilter(int pageIndex = 1, int pageSize = 50)
    {
        this.PageIndex = (pageIndex < 1) ? 1 : pageIndex;
        this.PageSize = (pageSize > 50) ? 50 : pageSize;
    }
}