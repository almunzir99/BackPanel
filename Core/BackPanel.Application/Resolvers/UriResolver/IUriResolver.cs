using BackPanel.Application.DTOs.Filters;

namespace BackPanel.Application.Resolvers.UriResolver;

public interface IUriResolver
{
    Uri GetPageUri(PaginationFilter filter, string route);
    string GetBaseUri();
}