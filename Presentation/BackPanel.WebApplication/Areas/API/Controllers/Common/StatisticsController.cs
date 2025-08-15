using BackPanel.Application;
using BackPanel.Application.DTOs.Wrapper;
using BackPanel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Areas.API.Controllers.Common;

[Authorize]
[ApiController]
[Route("api/statistics")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _service;

    public StatisticsController(IStatisticsService service)
    {
        _service = service;
    }
    [HttpGet()]
    public async Task<IActionResult> GetStatisticsController()
    {
          var result = await _service.GetCounters();
            var response = new Response<StatisticsDto>(data:result,message:"data retreived successfully !");
            return Ok(response);
    }
}