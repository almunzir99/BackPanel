using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Helpers;
using BackPanel.Application.Resolvers.UriResolver;
using BackPanel.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BackPanel.WebApplication.Areas.API.Controllers.Common;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = "ADMIN")]
public class RolesController : ControllerBase
{
    private readonly IUriResolver _uriResolver;
    private readonly RoleManager<AppRole> _roleManager;

    public RolesController(IUriResolver uriResolver, RoleManager<AppRole> roleManager)
    {
        _uriResolver = uriResolver;
        _roleManager = roleManager;
    }

    public string PermissionTitle => "RolesPermissions";

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] ListFilter filter)
    {
        var title = Request.Query["title"].ToString();
        var orderBy = Request.Query["orderBy"].ToString();
        var ascendingRaw = Request.Query["ascending"].ToString();
        var ascending = bool.TryParse(ascendingRaw, out var asc) && asc;

        var query = _roleManager.Roles.AsQueryable();
        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(x => x.Name != null && x.Name.Contains(title));

        query = (orderBy?.ToLowerInvariant()) switch
        {
            "title" => ascending ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),
            "createdat" => ascending ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
            _ => ascending ? query.OrderBy(x => x.LastUpdate) : query.OrderByDescending(x => x.LastUpdate)
        };

        var totalRecords = await query.CountAsync();
        var roles = await query
            .Skip((filter.PaginationFilter.PageIndex - 1) * filter.PaginationFilter.PageSize)
            .Take(filter.PaginationFilter.PageSize)
            .ToListAsync();

        var data = roles.Select(x => new RoleDto
        {
            Id = x.Id,
            Title = x.Name,
            Status = x.Status,
            CreatedAt = x.CreatedAt,
            LastUpdate = x.LastUpdate,
        }).ToList();

        if (Request.Path.Value == null)
            return BadRequest(new Response<string>(success: false, message: "Invalid request path"));

        return Ok(PaginationHelper.CreatePagedResponse(data, filter.PaginationFilter, _uriResolver, totalRecords, Request.Path.Value));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> SingleAsync(int id)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);
        if (role == null)
            return BadRequest(new Response<RoleDto>(success: false, errors: new List<string> { "Role not found" }));

        return Ok(new Response<RoleDto>(data: new RoleDto
        {
            Id = role.Id,
            Title = role.Name,
            Status = role.Status,
            CreatedAt = role.CreatedAt,
            LastUpdate = role.LastUpdate,
        }));
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(RoleDtoRequest body)
    {
        if (string.IsNullOrWhiteSpace(body.Title))
            return BadRequest(new Response<RoleDto>(success: false, errors: new List<string> { "Role title is required" }));

        var role = new AppRole
        {
            Name = body.Title,
            NormalizedName = body.Title.ToUpperInvariant(),
            CreatedAt = DateTime.UtcNow,
            LastUpdate = DateTime.UtcNow,
        };

        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
            return BadRequest(new Response<RoleDto>(success: false, errors: result.Errors.Select(x => x.Description).ToList()));

        return Ok(new Response<RoleDto>(data: new RoleDto
        {
            Id = role.Id,
            Title = role.Name,
            Status = role.Status,
            CreatedAt = role.CreatedAt,
            LastUpdate = role.LastUpdate,
        }));
    }

    [HttpPost("all")]
    public async Task<IActionResult> PostAllAsync(List<RoleDtoRequest> list)
    {
        foreach (var item in list)
        {
            if (string.IsNullOrWhiteSpace(item.Title))
                continue;
            var exists = await _roleManager.RoleExistsAsync(item.Title);
            if (exists)
                continue;
            await _roleManager.CreateAsync(new AppRole
            {
                Name = item.Title,
                NormalizedName = item.Title.ToUpperInvariant(),
                CreatedAt = DateTime.UtcNow,
                LastUpdate = DateTime.UtcNow,
            });
        }

        return Ok(new Response<string>(success: true, message: "data created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id, RoleDtoRequest body)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
            return BadRequest(new Response<RoleDto>(success: false, errors: new List<string> { "Role not found" }));

        role.Name = body.Title;
        role.NormalizedName = body.Title?.ToUpperInvariant();
        role.LastUpdate = DateTime.UtcNow;
        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded)
            return BadRequest(new Response<RoleDto>(success: false, errors: result.Errors.Select(x => x.Description).ToList()));

        return Ok(new Response<RoleDto>(data: new RoleDto
        {
            Id = role.Id,
            Title = role.Name,
            Status = role.Status,
            CreatedAt = role.CreatedAt,
            LastUpdate = role.LastUpdate,
        }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
            return BadRequest(new Response<RoleDto>(success: false, errors: new List<string> { "Role not found" }));

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
            return BadRequest(new Response<RoleDto>(success: false, errors: result.Errors.Select(x => x.Description).ToList()));

        return Ok(new Response<string>(success: true, message: "Item Deleted Successfully"));
    }

    [HttpGet("export/{type}")]
    public IActionResult Export(string type)
    {
        return BadRequest(new Response<string>(success: false, message: "Role export is not implemented in Identity mode yet"));
    }
}