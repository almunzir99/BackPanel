using BackPanel.Application.DTOs.Filters;

namespace BackPanel.Application.Interfaces;

public interface IUriService
{
    Uri GetPageUri(PaginationFilter filter, string route);
    string GetBaseUri();
}