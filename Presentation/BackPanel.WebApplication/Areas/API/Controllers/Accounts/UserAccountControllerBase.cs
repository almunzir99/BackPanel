using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.Extensions;
using BackPanel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Accounts;

/// <summary>
/// Abstract base controller for account-scoped endpoints (auth, profile, password)
/// for any user type (Admin, Customer, etc.).
///
/// To add a new user type's account controller:
///   1. Create XxxAccountController : UserAccountControllerBase&lt;XxxDto, XxxDtoRequest&gt;
///   2. Inject IXxxService : IUserService and expose it via the abstract UserService property.
///   3. Override virtual methods only where the new type needs different behaviour.
/// </summary>
[Authorize]
[ApiController]
public abstract class UserAccountControllerBase<TDto, TDtoRequest> : ControllerBase
    where TDto : AppUserDto
    where TDtoRequest : AppUserDtoRequest
{
    protected abstract IUserService UserService { get; }

    // ── Authenticate ─────────────────────────────────────────────────────────

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public virtual async Task<IActionResult> Authenticate([FromBody] AuthenticationModel model)
    {
        try
        {
            var result = await UserService.AuthenticateAsync(model);
            return Ok(new Response<AppUserDto>(data: result, message: "logged in successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<AppUserDto>(success: false, message: "logging in failed", errors: new[] { e.Message }));
        }
    }

    // ── Register ─────────────────────────────────────────────────────────────

    [Authorize(Roles = "ADMIN")]
    [HttpPost("register")]
    public virtual async Task<IActionResult> Register([FromBody] TDtoRequest body)
    {
        try
        {
            var user = await UserService.RegisterAsync(body);
            return Ok(new Response<AppUserDto>(data: user, message: "account created successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<AppUserDto>(success: false, message: "Registration failed", errors: new[] { e.Message }));
        }
    }

    // ── Profile ───────────────────────────────────────────────────────────────

    [Authorize]
    [HttpGet("profile")]
    public virtual async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var result = await UserService.GetProfileAsync(CurrentUserId);
            return Ok(new Response<AppUserDto>(data: result, success: true, message: "information fetched successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<AppUserDto>(success: false, message: "operation failed", errors: new[] { e.Message }));
        }
    }

    [Authorize]
    [HttpPut("profile")]
    public virtual async Task<IActionResult> UpdateProfile([FromBody] TDtoRequest body)
    {
        try
        {
            var result = await UserService.UpdateProfileAsync(CurrentUserId, body);
            return Ok(new Response<AppUserDto>(data: result, success: true, message: "profile updated successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<AppUserDto>(success: false, message: "operation failed", errors: new[] { e.Message }));
        }
    }

    // ── Password Reset ────────────────────────────────────────────────────────

    [Authorize]
    [HttpPut("profile/password-reset")]
    public virtual async Task<IActionResult> PasswordReset([FromBody] PasswordRecoveryRequest request)
    {
        try
        {
            await UserService.ResetPasswordAsync(CurrentUserId, request.OldPassword, request.NewPassword);
            return Ok(new Response<AppUserDto>(success: true, message: "password reset successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<AppUserDto>(success: false, message: "operation failed", errors: new[] { e.Message }));
        }
    }

    // ── Password Recovery ─────────────────────────────────────────────────────

    [AllowAnonymous]
    [HttpGet("password-recovery/request")]
    public virtual async Task<IActionResult> RequestPasswordRecovery([FromQuery] string email)
    {
        await UserService.RequestPasswordRecoveryAsync(email);
        return Ok(new Response<string>(success: true, data: null, message: "Password code sent successfully to email"));
    }

    [AllowAnonymous]
    [HttpGet("password-recovery/validate-code")]
    public virtual async Task<IActionResult> ValidateCode([FromQuery] string email, [FromQuery] int code)
    {
        var result = await UserService.ValidatePasswordRecoveryCodeAsync(email, code);
        return Ok(new Response<EmailRecoveryRequest>(success: true, data: result, message: "Code is validated successfully"));
    }

    [AllowAnonymous]
    [HttpGet("password-recovery/recover")]
    public virtual async Task<IActionResult> RecoverPassword([FromQuery] int userId, [FromQuery] string password)
    {
        await UserService.RecoverPasswordAsync(userId, password);
        return Ok(new Response<string>(success: true, data: null, message: "password recovered successfully"));
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    protected int CurrentUserId =>
        int.Parse(HttpContext.User.GetClaimValue("id"));
}
