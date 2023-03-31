using BackPanel.Application.Attributes.Permissions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Controllers;

public class AdminsController : UserBaseController<Admin, AdminDto, AdminDtoRequest, IAdminService>
{
    public AdminsController(IAdminService service, IUriService uriService, INotificationService notificationService) :
        base(service, uriService, notificationService)
    {
    }
    [Permission(true, PermissionTypes.READ)]

    [HttpGet]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationFilter? filter = null,
        [FromQuery] string? title = "", [FromQuery] string? orderBy = "LastUpdate", Boolean ascending = true)
    {
        var actionResult = await base.GetAsync(filter, title, orderBy, ascending);
        if (actionResult is OkObjectResult okActionResult)
        {
            if (okActionResult?.Value is PagedResponse<IList<AdminDto>> { Data: { } } response)
                response.Data = response.Data.Where(c => !c.IsManager && c.Id != GetCurrentUserId()).ToList();
        }

        return actionResult;
    }
    [Permission(true, PermissionTypes.READ)]
    [HttpGet("activities")]
    public async Task<IActionResult> GetActivitiesAsync([FromQuery] PaginationFilter? filter = null)
    {
        try
        {
            var validFilter = (filter == null)
                      ? new PaginationFilter()
                      : new PaginationFilter(pageIndex: filter.PageIndex, pageSize: filter.PageSize);
            var result = await Service.ActivitiesListAsync(filter);
            var totalRecords = await Service.GetActivitiesTotalRecords();
            if (Request.Path.Value != null)
            {
                return Ok(PaginationHelper.CreatePagedResponse(result,
                    validFilter, UriService, totalRecords, Request.Path.Value));
            }

            var response = new Response<string>(message: "Operation Failed because Request.Path.Value == null");
            return BadRequest(response);
        }
        catch (Exception e)
        {
            var response = new Response<string>(success: false, errors: new List<string>() { e.Message });
            return BadRequest(response);
        }
    }
    [Permission(true, PermissionTypes.READ)]
     [HttpGet("{userId}/activities")]
    public async Task<IActionResult> GetAdminActivitiesAsync(int userId,[FromQuery] PaginationFilter? filter = null)
    {
        try
        {
            var validFilter = (filter == null)
                      ? new PaginationFilter()
                      : new PaginationFilter(pageIndex: filter.PageIndex, pageSize: filter.PageSize);
            var result = await Service.AdminActivitiesListAsync(userId,filter);
            var totalRecords = await Service.GetActivitiesTotalRecords(c => c.AdminId == userId);
            if (Request.Path.Value != null)
            {
                return Ok(PaginationHelper.CreatePagedResponse(result,
                    validFilter, UriService, totalRecords, Request.Path.Value));
            }

            var response = new Response<string>(message: "Operation Failed because Request.Path.Value == null");
            return BadRequest(response);
        }
        catch (Exception e)
        {
            var response = new Response<string>(success: false, errors: new List<string>() { e.Message });
            return BadRequest(response);
        }
    }

    public override string PermissionTitle => "AdminsPermissions";
    protected override string Type => "ADMIN";
}