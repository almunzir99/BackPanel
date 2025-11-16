using BackPanel.SMTP.Interfaces;
using BackPanel.SMTP.Models;
using BackPanel.SMTP.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BackPanel.SMTP.DI;

public static class RegisterWithDependencyInjection
{
    public static void RegisterRequiredSmtpServices(this IServiceCollection services, SmtpConfigurationModel stmpConfiguration)
    {
        services.AddScoped<ISmtpService, SmtpService>(option =>
        {
            return new SmtpService(
                host: stmpConfiguration.Host!,
                port: stmpConfiguration.Port ?? 0,
                username: stmpConfiguration.Username!,
                password: stmpConfiguration.Password!);
        });
    }
}