using System.Linq.Expressions;
using BackPanel.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace BackPanel.Application.Interfaces;

public interface IRepositoryBase<TEntity> where TEntity : EntityBase
{
    Task<IList<TEntity>> ListAsync();
    Task<TEntity> SingleAsync(int id);
    Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> CreateAsync(TEntity newItem);
    Task<TEntity> UpdateAsync(int id, TEntity newItem);
    Task<TEntity> UpdateAsync(int id, JsonPatchDocument<TEntity> newItem);
    Task DeleteAsync(int id);
    Task<int> GetTotalRecords();
    IQueryable<TEntity> IncludeableDbSet {get; set;}
    Task<int> Complete();
}