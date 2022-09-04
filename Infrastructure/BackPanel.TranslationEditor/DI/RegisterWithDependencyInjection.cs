using BackPanel.FilesManager.Interfaces;
using BackPanel.TranslationEditor.Interfaces;
using BackPanel.TranslationEditor.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BackPanel.TranslationEditor.DI;

public static class RegisterWithDependencyInjection
{
    public static void ImplementPathProviderToTranslationService<T>(this IServiceCollection services)
        where T : class, IPathProvider
    {
        services.AddScoped<IPathProvider, T>();
    }
    public static void RegisterRequiredTranslationEditorServices(this IServiceCollection services)
    {
        services.AddScoped<ITranslationEditorService, TranslationEditorService>();
    }
}