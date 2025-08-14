using System.Text;
using BackPanel.Application.DTOs;
using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Interfaces;
using BackPanel.Application.Resolvers.UriResolver;
using BackPanel.Application.Resolvers.UserResolver;
using BackPanel.Application.Services;
using BackPanel.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BackPanel.Application.DI;
public static class RegisterWithDependencyInjection
{
    public static void RegisterRequiredApplicationService(this IServiceCollection services)
    {
        services.AddScoped<IStatisticsService, StatisticsService>();
    }

    public static void RegisterApplicationCQRS(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateCommandBase<DtoBase, DtoBase>).Assembly)
        );
    }

    public static void RegisterResolvers(this IServiceCollection services, Func<IServiceProvider, IUriResolver> implementationFactory)
    {
        services.AddScoped<IUserResolver, UserResolver>();
        services.AddScoped(implementationFactory);
    }

    public static void RegisterJwtConfiguration(this IServiceCollection services, string secretKey)
    {
        var key = Encoding.ASCII.GetBytes(secretKey);
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.RequireHttpsMetadata = false;
            o.TokenValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(key), ValidateIssuer = false, ValidateAudience = false };
        });
    }
}