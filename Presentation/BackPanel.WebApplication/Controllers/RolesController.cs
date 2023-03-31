using BackPanel.Application.Attributes.Permissions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ApiController<Role, RoleDto,RoleDtoRequest, IRolesService>
{
    public override string PermissionTitle => "RolesPermissions";

    public RolesController(IRolesService service, IUriService uriService) : base(service, uriService)
    {
    }
    [Permission(false, PermissionTypes.READ)]
    public override async  Task<IActionResult> SingleAsync(int id) {
        var result = await base.SingleAsync(id);
        return result;
    }
}