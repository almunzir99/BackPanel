using BackPanel.Application.Interfaces;
using BackPanel.FilesManager.Interfaces;

namespace BackPanel.WebApplication.implementation;

public class PathProvider : IPathProvider
{
    private readonly IUriService _uriService;
    private readonly IWebHostEnvironment _environment;
    public PathProvider(IUriService uriService, IWebHostEnvironment environment)
    {
        _uriService = uriService;
        _environment = environment;
    }
    public string GetRootPath() => _environment.WebRootPath;

    public string GetBaseUrl() => _uriService.GetBaseUri();
}