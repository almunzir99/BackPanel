using BackPanel.Application.DTOs.Wrapper;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;

namespace BackPanel.WebApplication.Middlewares
{
    public class GlobalErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                Log.Error("Request failure {@Route}, {@Error}, {@Date}",
                    context.Request.GetDisplayUrl(),
                    e,
                    DateTime.Now
                );
                // create error response 
                  var response = new Response<string>(success: false, message: "operation failed, check errors below",
                errors: new[] { e.Message });
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(response);
                
            }
        }
    }
}