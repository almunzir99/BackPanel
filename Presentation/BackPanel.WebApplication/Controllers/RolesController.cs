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
    private IRolesService _roleService;
    public override string PermissionTitle => "RolesPermissions";

    public RolesController(IRolesService service, IUriService uriService, IRolesService roleService) : base(service, uriService)
    {
        _roleService = roleService;
    }
}