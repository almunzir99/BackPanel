using System.Text;
using BackPanel.Application.Attributes.Permissions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Extensions;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.WebApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.WebApplication.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class ApiController<TEntity, TDto, TDtoRequest, TService> : ControllerBase, IApiController<TEntity, TDto, TDtoRequest, TService>
where TEntity : EntityBase where TDto : DtoBase where TService : IServicesBase<TEntity, TDto, TDtoRequest>
{
    protected readonly TService Service;
    protected readonly IUriService UriService;
    public abstract string PermissionTitle { get; }

    protected ApiController(TService service, IUriService uriService)
    {
        Service = service;
        UriService = uriService;
    }
    [Permission(true, PermissionTypes.READ)]
    [HttpGet]
    public virtual async Task<IActionResult> GetAsync(
        [FromQuery] PaginationFilter? filter = null,
        [FromQuery] string? title = "",
        [FromQuery] string? orderBy = "LastUpdate",
        [FromQuery] bool ascending = true,
        [FromQuery] IList<SearchExpressionDtoRequest>? expressions = null
        )
    {
        var validFilter = (filter == null)
            ? new PaginationFilter()
            : new PaginationFilter(pageIndex: filter.PageIndex, pageSize: filter.PageSize);
        var result = await Service.List(filter, title, orderBy!, ascending,expressions).ToListAsync();
        var totalRecords = result.Count;
        if (Request.Path.Value != null)
        {
            return Ok(PaginationHelper.CreatePagedResponse(result,
                validFilter, UriService, totalRecords, Request.Path.Value));
        }

        var response = new Response<string>(message: "Operation Failed because Request.Path.Value == null");
        return BadRequest(response);
    }
    [Permission(true, PermissionTypes.READ)]
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> SingleAsync(int id)
    {
        try
        {
            var result = await Service.SingleAsync(id);
            var response = new Response<TDto>(data: result);
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, errors: new List<string>() { e.Message });
            return BadRequest(response);
        }
    }
    [Permission(true, PermissionTypes.CREATE)]

    [HttpPost]
    public virtual async Task<IActionResult> PostAsync(TDtoRequest body)
    {
        try
        {
            int currentUserId = int.Parse(HttpContext.User.GetClaimValue("id"));
            var result = await Service.CreateAsync(body);
            var response = new Response<TDto>(data: result);
            //create Activity
            if (!IsAnonymous("PostAsync"))
                await Service.CreateActivity(currentUserId, result.Id, "Create");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, errors: new List<string>() { e.Message });
            return BadRequest(response);
        }
    }
    [Permission(true, PermissionTypes.CREATE)]
    [HttpPost("all")]
    public async Task<IActionResult> PostAllAsync(IList<TDtoRequest> list)
    {
        try
        {
            await Service.CreateAllAsync(list);
            var response = new Response<TDto>(success: true, message: "data created successfully");
            return Ok(response);
        }
        catch (System.Exception e)
        {
            var response = new Response<TDto>(success: false, errors: new List<string>() { e.Message });
            return BadRequest(response);
        }
    }
    [Permission(true, PermissionTypes.UPDATE)]
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> PutAsync(int id, TDtoRequest body)
    {
        try
        {
            var result = await Service.UpdateAsync(id, body);
            var response = new Response<TDto>(data: result);
            //create Activity
            int currentUserId = int.Parse(HttpContext.User.GetClaimValue("id"));
            await Service.CreateActivity(currentUserId, id, "Update");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, errors: new List<string>() { e.Message });
            return BadRequest(response);
        }
    }
    [Permission(true, PermissionTypes.DETELE)]
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            await Service.DeleteAsync(id);
            var response = new Response<TDto>(message: "Item Deleted Successfully");
            //create Activity
            int currentUserId = int.Parse(HttpContext.User.GetClaimValue("id"));
            await Service.CreateActivity(currentUserId, id, "Delete");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, errors: new List<string>() { e.Message });
            return BadRequest(response);
        }
    }
    [Permission(true, PermissionTypes.UPDATE)]
    [HttpPatch("{id}")]
    public virtual async Task<IActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<TEntity> body)
    {
        try
        {
            var result = await Service.UpdateAsync(id, body);
            var response = new Response<TDto>(data: result);
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, errors: new List<string>() { e.Message });
            return BadRequest(response);
        }
    }
    [HttpGet("export/excel")]
    public virtual async Task<IActionResult> ExportToExcel()
    {
        var content = await this.Service.ExportToExcel();
        var result = new FileContentResult(content,
            "application/xls")
        {
            FileDownloadName = "data.xls",
        };
        return result;
    }
    [HttpGet("export/pdf")]
    public virtual async Task<IActionResult> ExportToPdf()
    {
        var content = await this.Service.ExportToPdf();
        var result = new FileContentResult(content,
            "application/pdf")
        {
            FileDownloadName = "data.pdf",
        };
        return result;
    }
    [HttpGet("active")]
    public async Task<IActionResult> ActiveToggleAsync(int id)
    {
        try
        {
            await Service.ActiveToggleAsync(id);
            var response = new Response<TDto>(message: "item activation toggled successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, errors: new List<string>() { e.Message });
            return BadRequest(response);
        }
    }
    private bool IsAnonymous(string name)
    {
        var type = this.GetType();
        var targetMethod = Array.Find(type.GetMethods(), c => c.Name == name);
        var isAnonymous = targetMethod?.GetCustomAttributes(true).SingleOrDefault(c => c.GetType().Name == "AllowAnonymousAttribute");
        return isAnonymous != null;
    }
    protected int GetCurrentUserId()
    {
        int currentUserId = int.Parse(HttpContext.User.GetClaimValue("id"));
        return currentUserId;
    }
    protected string GetCurrentUserType()
    {
        string type = HttpContext.User.GetClaimValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
        return type;
    }
}