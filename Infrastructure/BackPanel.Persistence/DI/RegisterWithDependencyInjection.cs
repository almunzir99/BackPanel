using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.Persistence.Database;
using BackPanel.Persistence.Repository;
using BackPanel.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackPanel.Persistence.DI;

public static class RegisterWithDependencyInjection
{
    public static void RegisterDbContext<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(
            configuration.GetConnectionString("default")
        ));
        services.AddTransient<MapperHelper>();
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        var assembly = typeof(EntityBase).Assembly;
        foreach (var type in assembly.GetTypes())
        {
            if (typeof(EntityBase).IsAssignableFrom(type) && type.Name != "EntityBase" )
            {
                var method = typeof(RegisterWithDependencyInjection).GetMethod("RegisterRepositoriesWithDi");
                if (method != null)
                {
                    var genericMethod = method.MakeGenericMethod(type);
                    genericMethod.Invoke(null, new object?[] { services });
                }
            }
        }
    }

    public static void RegisterRepositoriesWithDi<T>(IServiceCollection services)
        where T : EntityBase
    {
        services.AddScoped<IRepositoryBase<T>, RepositoryBase<T>>();
    }
}