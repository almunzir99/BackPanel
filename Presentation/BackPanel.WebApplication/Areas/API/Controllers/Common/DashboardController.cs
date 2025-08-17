using BackPanel.Application;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.Features.Dashboard.Queries;
using BackPanel.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Common;

[Authorize]
[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("counters")]
    public async Task<IActionResult> GetStatisticsController()
    {
        var result = await _mediator.Send(new GetDashboardCountersQuery());
        var response = new Response<CountersDto>(data: result, message: "data retreived successfully !");
        return Ok(response);
    }
}