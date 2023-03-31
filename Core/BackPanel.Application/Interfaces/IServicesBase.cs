using System.Linq.Expressions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace BackPanel.Application.Interfaces;

public interface IServicesBase<TEntity, TDto, TDtoRequest> where TEntity : EntityBase where TDto : DtoBase
{
    Task<IList<TDto>> ListAsync(PaginationFilter? filter, IList<Func<TEntity, bool>>? conditions, string? title = "",
        string orderBy = "LastUpdate", Boolean ascending = true);

    Task<TDto> SingleAsync(int id);
    Task<TDto> CreateAsync(TDtoRequest newItem);
    Task<TDto> UpdateAsync(int id, TDtoRequest newItem);
    Task<TDto> UpdateAsync(int id, JsonPatchDocument<TEntity> newItem);
    Task DeleteAsync(int id);
    Task<Byte[]> ExportToExcel();
    Task<Byte[]> ExportToPdf();
    Task<int> GetTotalRecords(Expression<Func<TEntity, bool>>? predicate = null);
    Task CreateActivity(int userId, int rowId, string action);
    Task CreateAllAsync(IList<TDtoRequest> newItems);

}