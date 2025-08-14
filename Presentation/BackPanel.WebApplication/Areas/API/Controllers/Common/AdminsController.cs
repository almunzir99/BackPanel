using BackPanel.Application.Attributes.Permissions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Generic.Common.Queries;
using BackPanel.Application.Helpers;
using BackPanel.Application.Resolvers.UriResolver;
using BackPanel.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Common;

[Route("api/admins")]
public class AdminsController : ApiController<Admin, AdminDto, AdminDtoRequest>
{
    public AdminsController(IUriResolver uriService, IMediator mediator) : base(uriService, mediator)
    {
    }

    [Permission(true, PermissionTypes.READ)]
    [HttpGet]
    public override async Task<IActionResult> GetAsync([FromQuery] ListFilter filter)
    {
        try
        {
            // Exclude the managers for the list
            filter.SearchExpressions.Add(new SearchExpressionDtoRequest
            {
                PropName = nameof(AdminDto.IsManager),
                PropValue = "false",
                Operator = Domain.Enums.ComparisonOperator.Equal
            });
            var result = await Mediator.Send(new GetAllQueryBase<AdminDto>(filter));
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
    public override string PermissionTitle => "AdminsPermissions";
}