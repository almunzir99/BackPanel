using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackPanel.WebApplication.Middlewares;

namespace BackPanel.WebApplication.Extensions
{
    public static class RegisterMiddlewaresWithDependancyInjection
    {
        public static  void RegisterMiddlewares(this IServiceCollection services) {
            services.AddTransient<GlobalErrorHandlingMiddleware>();
        }
    }
}