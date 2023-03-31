using BackPanel.Application.Extensions;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

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
        {   var isManager = context.HttpContext.User.GetClaimValue("type") == "manager";
            if (_enabled && !isManager)
            {
                var permission = await GetPermission(context);

                var forbiddenContentResult = new ContentResult()
                {
                    Content = "Access Denied, You don't have Permission for this operation",
                    StatusCode = 403,
                    ContentType = "application/json"
                };
                if (permission == null)
                {
                    context.Result = new ContentResult()
                    {
                        Content = "Permission is not Implemented",
                        StatusCode = 400,
                        ContentType = "application/json"
                    };
                    return;
                }
                switch (_type)
                {
                    case PermissionTypes.CREATE:
                        if (!permission.Create)
                        {
                            context.Result = forbiddenContentResult;
                            return;
                        }
                        break;
                    case PermissionTypes.READ:
                        if (!permission.Read)
                        {
                            context.Result = forbiddenContentResult;
                            return;
                        }
                        break;
                    case PermissionTypes.UPDATE:
                        if (!permission.Update)
                        {
                            context.Result = forbiddenContentResult;
                            return;
                        }
                        break;
                 case PermissionTypes.DETELE:
                        if (!permission.Delete)
                        {
                            context.Result = forbiddenContentResult;
                            return;
                        }
                        break;
                    default:
                        break;
                }
            }
            await next();
        }
        private static async Task<Permission?> GetPermission(ActionExecutingContext context)
        {
            // Get PermissionTitle Value from controller
            var controllerType = context.Controller.GetType();
            var permissionProp = controllerType.GetProperties().Single(c => c.Name == "PermissionTitle");
            if(permissionProp == null) return null;
            var  PermissionTitle =  permissionProp.GetValue(context.Controller) as String;
            // get title
            var title = context.HttpContext.User.GetClaimValue("admin_role");
            // get role Service
            var roleService = context.HttpContext.RequestServices.GetService<IRolesService>();
            var role = await roleService!.GetRoleByTitle(title);
        if (role != null)
        {
            var roleType = role.GetType();
            var permission = roleType.GetProperties().Single(c => c.Name == PermissionTitle);
            if (permission == null)
                return null;
            var result = permission.GetValue(role) as Permission;
            return result;
        }
        else
        {
            return null;
        }
    }
    }