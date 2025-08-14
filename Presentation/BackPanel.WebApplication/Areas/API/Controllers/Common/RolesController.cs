using BackPanel.Application.Attributes.Permissions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Resolvers.UriResolver;
using BackPanel.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Common;

[ApiController]
[Route("api/roles")]
public class RolesController : ApiController<Role, RoleDto, RoleDtoRequest>
{
    public RolesController(IUriResolver uriService, IMediator mediator) : base(uriService, mediator)
    {
    }

    public override string PermissionTitle => "RolesPermissions";


    [Permission(false, PermissionTypes.READ)]
    public override async Task<IActionResult> SingleAsync(int id)
    {
        var result = await base.SingleAsync(id);
        return result;
    }
}