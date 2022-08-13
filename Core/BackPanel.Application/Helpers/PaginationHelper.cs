using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.Interfaces;

namespace BackPanel.Application.Helpers;

public class PaginationHelper
{
    public static PagedResponse<IList<T>> CreatePagedResponse<T>(IList<T> data, PaginationFilter filter, IUriService _uriSerivce, int totalRecords, string route)
    {
        var totalPages = (int)(Math.Ceiling((totalRecords / (filter.PageSize * 1.0))));
        return new PagedResponse<IList<T>>(data: data,
            pageSize: filter.PageSize,
            pageIndex: filter.PageIndex,
            totalPages: totalPages,
            totalRecords: totalRecords,
            firstPage: _uriSerivce.GetPageUri(new PaginationFilter(pageSize: filter.PageSize, pageIndex: 1), route),
            lastPage: _uriSerivce.GetPageUri(new PaginationFilter(pageSize: filter.PageSize, pageIndex: totalPages), route),
            nextPage: ((filter.PageIndex == totalPages) ? null : _uriSerivce.GetPageUri(new PaginationFilter(pageSize: filter.PageSize, pageIndex: filter.PageIndex + 1), route)),
            previousPage: (filter.PageIndex == 1) ? null : _uriSerivce.GetPageUri(new PaginationFilter(pageSize: filter.PageSize, pageIndex: filter.PageIndex - 1), route));


    }
}