using BackPanel.Application.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackPanel.Application.Attributes.Permissions;
[AttributeUsage(AttributeTargets.Method)]
public class PermissionAttribute : Attribute, IAsyncActionFilter
{
    private readonly bool _enabled;
    private readonly PermissionTypes _type;

    public PermissionAttribute(bool enabled, PermissionTypes type)
    {
        _enabled = enabled;
        _type = type;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next();
    }
}