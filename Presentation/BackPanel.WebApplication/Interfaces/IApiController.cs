using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BackPanel.WebApplication.Interfaces;

public interface IApiController<TEntity, TDto, TDtoRequest, TService>
    where TEntity : EntityBase where TDto : DtoBase where TService : IServicesBase<TEntity, TDto, TDtoRequest>
{
    Task<IActionResult> GetAsync(PaginationFilter? filter = null, string title = "",
        [FromQuery] string orderBy = "LastUpdate", Boolean ascending = true);

    Task<IActionResult> SingleAsync(int id);
    Task<IActionResult> PostAsync(TDtoRequest body);
    Task<IActionResult> PutAsync(int id, TDtoRequest body);
    Task<IActionResult> PostAllAsync(IList<TDtoRequest> items);

    Task<IActionResult> DeleteAsync(int id);
}