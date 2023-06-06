using BackPanel.Application.Attributes.Permissions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyInfosController : ApiController<CompanyInfo, CompanyInfoDto,CompanyInfoDtoRequest, ICompanyInfosService>
{
    public CompanyInfosController(ICompanyInfosService service, IUriService uriSerivce) : base(service, uriSerivce)
    {
    }
    [HttpGet("single")]
    public async Task<IActionResult> GetCompanyInfoAsync()
    {
        try
        {
            var result = await Service.GetCompanyInfoAsync();
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