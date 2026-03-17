using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Features.Business.Queries;
using BackPanel.Application.Resolvers.UriResolver;
using BackPanel.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Common;

[ApiController]
[Route("api/business")]
public class BusinessesController : ApiController<Business, BusinessDto, BusinessDtoRequest>
{
    public BusinessesController(IUriResolver uriService, IMediator mediator) : base(uriService, mediator)
    {
    }

    [HttpGet("single")]
    public async Task<IActionResult> GetBusinessAsync()
    {
        try
        {
            var result = await Mediator.Send(new GetBusinessQuery());
            var response = new Response<BusinessDto>(data: result);
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<string>(message: "Operation Failed", errors: new List<string> { e.Message });
            return BadRequest(response);
        }
    }
    public override string PermissionTitle => "BusinessesPermissions";
}
