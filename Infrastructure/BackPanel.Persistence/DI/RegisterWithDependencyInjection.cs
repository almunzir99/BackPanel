using BackPanel.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackPanel.Persistence.DI;

public static class RegisterWithDependencyInjection
{
    public static void RegisterDbContext<T>(this IServiceCollection services,IConfiguration configuration)
        where T : DbContext
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(
            configuration.GetConnectionString("default")
        ));
    }
}