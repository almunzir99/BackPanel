using BackPanel.FilesManager.Interfaces;
using BackPanel.FilesManager.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BackPanel.FilesManager.DI;

public static class RegisterWithDependencyInjection
{
    public static void RegisterRequiredFilesManagerServices(this IServiceCollection services)
    {
        services.AddScoped<IFilesManagerService, FilesManagerService>();
    }

    public static void ImplementPathProvider<T>(this IServiceCollection services)
        where T : class, IPathProvider
    {
        services.AddScoped<IPathProvider, T>();
    }
}