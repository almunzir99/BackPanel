using BackPanel.Application.Resolvers.UriResolver;
using BackPanel.FilesManager.Interfaces;

namespace BackPanel.WebApplication.implementation;

public class PathProvider : IPathProvider
{
    private readonly IUriResolver _uriService;
    private readonly IWebHostEnvironment _environment;
    public PathProvider(IUriResolver uriService, IWebHostEnvironment environment)
    {
        _uriService = uriService;
        _environment = environment;
    }
    public string GetRootPath() => _environment.WebRootPath;

    public string GetBaseUrl() => _uriService.GetBaseUri();
}