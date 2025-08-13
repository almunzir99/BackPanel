using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.Resolvers.UriResolver;
using Microsoft.AspNetCore.WebUtilities;

namespace BackPanel.Resolvers.UriResolver;

public class UriResolver : IUriResolver
{
    private readonly string _baseUri;

    public UriResolver(string baseUri)
    {
        _baseUri = baseUri;
    }
    public string GetBaseUri()
    {
        return this._baseUri;
    }
    public Uri GetPageUri(PaginationFilter filter, string route)
    {
        var endPoint = new Uri(string.Concat(_baseUri, route));
        if (endPoint == null) throw new Exception("enPoint shouldn't be null");
        var endPointWithParams = QueryHelpers.AddQueryString(endPoint.ToString(), "pageIndex", filter.PageIndex.ToString());
        endPointWithParams = QueryHelpers.AddQueryString(endPointWithParams, "pageSize", filter.PageSize.ToString());
        return new Uri(endPointWithParams);
    }
}