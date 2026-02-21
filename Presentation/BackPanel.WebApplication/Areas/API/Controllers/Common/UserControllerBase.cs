using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Extensions;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Application.Resolvers.UriResolver;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Common;

/// <summary>
/// Abstract base controller for any user type (Admin, Customer, etc.).
/// Mirrors the shape of <see cref="ApiController{TEntity,TDto,TDtoRequest}"/> but
/// is powered by <see cref="IUserService"/> instead of MediatR generic CQRS,
/// because AppUser does not extend EntityBase.
///
/// To add a new user type:
///   1. (Optional) Create a domain entity that extends AppUser via EF TPH.
///   2. Create TDto : AppUserDto  and  TDtoRequest : AppUserDtoRequest.
///   3. Create IXxxService : IUserService (add any extra methods you need).
///   4. Implement XxxService and register it in DI.
///   5. Create XxxController : UserControllerBase&lt;TDto, TDtoRequest&gt;,
///      inject IXxxService, IUriResolver and expose them via the abstract properties.
///      Override virtual methods only where the new type needs different behaviour.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class UserControllerBase<TDto, TDtoRequest> : ControllerBase
    where TDto : AppUserDto
    where TDtoRequest : AppUserDtoRequest
{
    public abstract string PermissionTitle { get; }

    /// <summary>
    /// Concrete controllers inject their specific service and expose it here.
    /// </summary>
    protected abstract IUserService UserService { get; }

    /// <summary>
    /// Concrete controllers inject IUriResolver and expose it here (used for pagination links).
    /// </summary>
    protected abstract IUriResolver UriResolver { get; }

    // ── List (paginated) ──────────────────────────────────────────────────────

    [Authorize(Roles = "ADMIN")]
    [HttpGet]
    public virtual async Task<IActionResult> GetAsync([FromQuery] ListFilter filter)
    {
        try
        {
            var result = await UserService.GetAllFilteredAsync(filter);
            if (Request.Path.Value != null)
                return Ok(PaginationHelper.CreatePagedResponse(
                    result.Item1, filter.PaginationFilter, UriResolver, result.Item2, Request.Path.Value));

            return BadRequest(new Response<string>(message: "Operation failed: Request.Path.Value is null"));
        }
        catch (Exception e)
        {
            return BadRequest(new Response<string>(success: false, errors: new List<string> { e.Message }));
        }
    }

    // ── Export to Excel ───────────────────────────────────────────────────────

    [Authorize(Roles = "ADMIN")]
    [HttpGet("export/excel")]
    public virtual async Task<IActionResult> ExportToExcel()
    {
        var content = await UserService.ExportToExcelAsync();
        return new FileContentResult(content, "application/xls")
        {
            FileDownloadName = "data.xls",
        };
    }

    // ── Single ────────────────────────────────────────────────────────────────

    [Authorize(Roles = "ADMIN")]
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> SingleAsync(int id)
    {
        try
        {
            var result = await UserService.GetByIdAsync(id);
            var response = new Response<AppUserDto>(data: result);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(new Response<AppUserDto>(success: false, errors: new List<string> { e.Message }));
        }
    }

    // ── Create ────────────────────────────────────────────────────────────────

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public virtual async Task<IActionResult> PostAsync([FromBody] TDtoRequest body)
    {
        try
        {
            var result = await UserService.RegisterAsync(body);
            var response = new Response<AppUserDto>(data: result);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(new Response<AppUserDto>(success: false, errors: new List<string> { e.Message }));
        }
    }

    // ── Update ────────────────────────────────────────────────────────────────

    [Authorize(Roles = "ADMIN")]
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> PutAsync(int id, [FromBody] TDtoRequest body)
    {
        try
        {
            var result = await UserService.UpdateProfileAsync(id, body);
            var response = new Response<AppUserDto>(data: result);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(new Response<AppUserDto>(success: false, errors: new List<string> { e.Message }));
        }
    }

    // ── Delete ────────────────────────────────────────────────────────────────

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            await UserService.DeleteAsync(id);
            var response = new Response<AppUserDto>(message: "User deleted successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(new Response<AppUserDto>(success: false, errors: new List<string> { e.Message }));
        }
    }

    // ── Toggle Active ─────────────────────────────────────────────────────────

    [Authorize(Roles = "ADMIN")]
    [HttpGet("active")]
    public virtual async Task<IActionResult> ActiveToggleAsync(int id)
    {
        try
        {
            await UserService.ToggleActiveAsync(id);
            var response = new Response<AppUserDto>(message: "User activation toggled successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(new Response<AppUserDto>(success: false, errors: new List<string> { e.Message }));
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    protected int CurrentUserId
    {
        get
        {
            int currentUserId = int.Parse(HttpContext.User.GetClaimValue("id"));
            return currentUserId;
        }
    }
}
