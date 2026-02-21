using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Interfaces;
using BackPanel.Application.Resolvers.UriResolver;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Common;

/// <summary>
/// Manages admin users (list, create, update, delete, activate, export).
/// Extends UserControllerBase which mirrors the ApiController shape.
///
/// All authentication endpoints (login, password recovery, profile) live in
/// AdminAccountController — this controller is purely for admin management
/// from the dashboard.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AdminsController : UserControllerBase<AppUserDto, AppUserDtoRequest>
{
    public override string PermissionTitle => "ADMINS";

    private readonly IUserService _userService;
    private readonly IUriResolver _uriResolver;

    protected override IUserService UserService => _userService;
    protected override IUriResolver UriResolver => _uriResolver;

    public AdminsController(IUserService userService, IUriResolver uriResolver)
    {
        _userService = userService;
        _uriResolver = uriResolver;
    }
}
