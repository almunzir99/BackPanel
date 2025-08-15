using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Features.CompanyInfo.Queries;
using BackPanel.Application.Resolvers.UriResolver;
using BackPanel.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Common;

[ApiController]
[Route("api/company-info")]
public class CompanyInfosController : ApiController<CompanyInfo, CompanyInfoDto, CompanyInfoDtoRequest>
{
    public CompanyInfosController(IUriResolver uriService, IMediator mediator) : base(uriService, mediator)
    {
    }

    [HttpGet("single")]
    public async Task<IActionResult> GetCompanyInfoAsync()
    {
        try
        {
            var result = await Mediator.Send(new GetCompanyInfoQuery());
            var response = new Response<CompanyInfoDto>(data: result);
            return Ok(response);
        }
        catch (Exception e)
        {
            var response = new Response<string>(message: "Operation Failed", errors: new List<string> { e.Message });
            return BadRequest(response);
        }
    }
    public override string PermissionTitle => "CompanyInfosPermissions";
}