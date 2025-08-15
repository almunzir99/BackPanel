using System.Text;
using BackPanel.Application.Attributes.Permissions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.Extensions;
using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Queries;
using BackPanel.Application.Helpers;
using BackPanel.Application.Resolvers.UriResolver;
using BackPanel.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Common;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class ApiController<TEntity, TDto, TDtoRequest> : ControllerBase
where TEntity : EntityBase where TDto : DtoBase
{
    protected readonly IUriResolver UriResolver;
    public abstract string PermissionTitle { get; }
    protected readonly IMediator Mediator;

    protected ApiController(IUriResolver uriResolver, IMediator mediator)
    {
        UriResolver = uriResolver;
        Mediator = mediator;
    }
    [Permission(true, PermissionTypes.READ)]
    [HttpGet]
    public virtual async Task<IActionResult> GetAsync(
        [FromQuery] ListFilter filter
        )
    {
        try
        {
            var result = await Mediator.Send(new GetAllQueryBase<TDto>(filter));
            if (Request.Path.Value != null)
            {
                return Ok(PaginationHelper.CreatePagedResponse(result.Item1,
                    filter.PaginationFilter, UriResolver, result.Item2, Request.Path.Value));
            }
            var response = new Response<string>(message: "Operation Failed because Request.Path.Value == null");
            return BadRequest(response);
        }
        catch (Exception e)
        {

            var response = new Response<string>(message: "Operation Failed because Request.Path.Value == null");
            return BadRequest(response);
        }

    }
    [Permission(true, PermissionTypes.READ)]
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> SingleAsync(int id)
    {

        try
        {
            var result = await Mediator.Send(new GetByIdQueryBase<TDto>(id));
            var response = new Response<TDto>(data: result);
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, errors: new List<string>() { e.Message });
            return BadRequest(response);
        }
    }
    [HttpPost]
    public virtual async Task<IActionResult> PostAsync(TDtoRequest body)
    {
        try
        {
            var command = new CreateCommandBase<TDtoRequest, TDto>(body);
            var result = await Mediator.Send(command);
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
    [HttpPost("all")]
    public async Task<IActionResult> PostAllAsync(List<TDtoRequest> list)
    {
        try
        {
            await Mediator.Send(new CreateBulkCommandBase<TDtoRequest>(list));
            var response = new Response<TDto>(success: true, message: "data created successfully");
            return Ok(response);
        }
        catch (Exception e)
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
            var result = await Mediator.Send(new UpdateCommandBase<TDtoRequest, TDto>(id, body));
            var response = new Response<TDto>(data: result);
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
            await Mediator.Send(new DeleteCommandBase<TEntity>(id));
            var response = new Response<TDto>(message: "Item Deleted Successfully");
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
        var content = await Mediator.Send(new ExportToExcelQueryBase<TEntity>());
        var result = new FileContentResult(content,
            "application/xls")
        {
            FileDownloadName = "data.xls",
        };
        return result;

    }

    [HttpGet("active")]
    public async Task<IActionResult> ActiveToggleAsync(int id)
    {
        try
        {
            await Mediator.Send(new ToggleActiveCommandBase<TEntity>(id));
            var response = new Response<TDto>(message: "item activation toggled successfully");
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<TDto>(success: false, errors: new List<string>() { e.Message });
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