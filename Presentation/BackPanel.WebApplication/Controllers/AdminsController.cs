using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.DTOsRequests;
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

    [HttpGet]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationFilter? filter = null,
        [FromQuery] string? title = "", [FromQuery] string? orderBy = "LastUpdate", Boolean ascending = true)
    {
        var actionResult = await base.GetAsync(filter, title, orderBy, ascending);
        if (actionResult is OkObjectResult okActionResult)
        {
            if (okActionResult?.Value is PagedResponse<IList<AdminDto>> { Data: { } } response)
                response.Data = response.Data.Where(c => c.IsManager == false && c.Id != GetCurrentUserId()).ToList();
        }

        return actionResult;
    }

    public override string PermissionTitle => "AdminsPermissions";
    protected override string Type => "ADMIN";
}