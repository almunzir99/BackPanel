using System.Text;
using BackPanel.Application.Interfaces;
using BackPanel.Application.Services;
using BackPanel.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BackPanel.Application.DI;

public static class RegisterWithDependencyInjection
{
    public static void RegisterRequiredApplicationService(this IServiceCollection services)
    {
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IRolesService, RolesService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IMessageService, MessagesService>();
        services.AddScoped<IStatisticsService, StatisticsService>();
    }
    public static void ImplementUriService(this IServiceCollection services,
        Func<IServiceProvider, IUriService> implementationFactory)
    {
        services.AddScoped<IUriService>(implementationFactory);
    }
    public static void RegisterJwtConfiguration(this IServiceCollection services, string secretKey)
    {
        var key = Encoding.ASCII.GetBytes(secretKey);
        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            })
            ;
    }
}