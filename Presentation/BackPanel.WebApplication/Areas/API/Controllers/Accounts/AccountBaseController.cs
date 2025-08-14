using System.ComponentModel.DataAnnotations;
using BackPanel.Application.Attributes.Permissions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Extensions;
using BackPanel.Application.Generic.Accounts.Commands;
using BackPanel.Application.Generic.Accounts.Queries;
using BackPanel.Application.Resolvers.UriResolver;
using BackPanel.Domain.Entities;
using BackPanel.WebApplication.implementation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Accounts;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class
    AccountBaseController<TEntity, TDto, TDtoRequest> : ControllerBase
    where TEntity : UserEntityBase
    where TDto : UserDtoBase
    where TDtoRequest : UserBaseDtoRequest
{
    public abstract string PermissionTitle { get; }
    protected abstract string Type { get; }
    protected readonly IMediator Mediator;

    protected AccountBaseController(IUriResolver uriService, IMediator mediator)
    {
        Mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public virtual async Task<IActionResult> Authenticate(AuthenticationModel model)
    {
        try
        {
            var result = await Mediator.Send(new AuthenticateCommandBase<TEntity, TDto>(model));
            var response = new Response<TDto>(data: result, message: "you logged in successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, message: "logging in failed, check errors below",
                errors: new[] { e.Message });
            return BadRequest(response);
        }
    }
    [Permission(true, PermissionTypes.CREATE)]
    [HttpPost("register")]
    public virtual async Task<IActionResult> Register(TDtoRequest body)
    {
        try
        {
            var user = await Mediator.Send(new RegisterCommandBase<TDtoRequest, TDto>(body));
            var response = new Response<TDto>(data: user, message: "account created successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, message: "Registration failed, check errors below",
                errors: new[] { e.Message });
            return BadRequest(response);
        }
    }

    [AllowAnonymous]
    [HttpGet("password/recovery-request")]
    public virtual async Task<IActionResult> PasswordRecoveryRequest([Required][FromQuery] string email)
    {
        try
        {
            var result = await Mediator.Send(new PasswordRecoveryRequestCommandBase<TEntity>(email));
            var response = new Response<TDto>(success: true, message: "request send successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, message: "operation failed, check errors below",
                errors: new[] { e.Message });
            return BadRequest(response);
        }
    }

    [AllowAnonymous]
    [HttpPost("password/recovery")]
    public virtual async Task<IActionResult> PasswordRecovery(PasswordRecoveryRequest request)
    {
        try
        {

            await Mediator.Send(new PasswordRecoverCommandBase<TEntity>(request));
            var response = new Response<TDto>(success: true, message: "password reset successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, message: "operation failed, check errors below",
                errors: new[] { e.Message });
            return BadRequest(response);
        }
    }

    [Authorize]
    [HttpPut("profile/password-reset")]
    public virtual async Task<IActionResult> PasswordReset(PasswordRecoveryRequest request)
    {
        if (CurrentUserType != Type)
            return StatusCode(403);
        try
        {

            await Mediator.Send(new PasswordResetCommandBase<TEntity>(CurrentUserId, request));
            var response = new Response<TDto>(success: true, message: "password reset successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, message: "operation failed, check errors below",
                errors: new[] { e.Message });
            return BadRequest(response);
        }
    }

    [Authorize]
    [HttpPut("profile/change-photo")]
    public virtual async Task<IActionResult> ChangePersonalPhoto(IFormFile file)
    {
        if (CurrentUserType != Type)
            return StatusCode(403);
        try
        {
            var id = CurrentUserId;
            var webFile = new WebFormFile(file, file.FileName);
            var result = await Mediator.Send(new ChangePersonalPhotoCommandBase<TEntity>(id, webFile));
            var response = new Response<string>(data: result, success: true, message: "personal photo updated successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, message: "operation failed, check errors below",
                errors: new[] { e.Message });
            return BadRequest(response);
        }
    }


    [Authorize]
    [HttpGet("profile")]
    public virtual async Task<IActionResult> GetCurrentUser()
    {
        if (CurrentUserType != Type)
            return StatusCode(403);
        try
        {

            var result = await Mediator.Send(new GetProfileQueryBase<TDto>(CurrentUserId));
            var response = new Response<TDto>(data: result, success: true, message: "information fetched successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, message: "operation failed, check errors below",
                errors: new[] { e.Message });
            return BadRequest(response);
        }
    }
    protected int CurrentUserId
    {
        get
        {
            int currentUserId = int.Parse(HttpContext.User.GetClaimValue("id"));
            return currentUserId;
        }
    }

    protected string CurrentUserType
    {
        get
        {
            string type = HttpContext.User.GetClaimValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            return type;
        }
    }
}