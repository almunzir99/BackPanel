using BackPanel.SMTP.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BackPanel.SMTP.DI;

public static class RegisterWithDependencyInjection
{
    public static void RegisterRequiredSmtpServices(this IServiceCollection services)
    {
        services.AddScoped<ISmtpService, SmtpService>();
    }
}