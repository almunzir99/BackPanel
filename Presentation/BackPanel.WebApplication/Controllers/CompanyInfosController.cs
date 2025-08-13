using BackPanel.Application.Attributes.Permissions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Features.CompanyInfo.Queries.GetCompanyInfo;
using BackPanel.Application.Resolvers.UriResolver;
using BackPanel.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
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
            var response = new Response<string>(success: false, errors: new List<string>() { e.Message });
            return BadRequest(response);
        }
    }
    public override string PermissionTitle => "CompanyInfosPermissions";
}