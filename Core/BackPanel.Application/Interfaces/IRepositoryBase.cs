using System.Linq.Expressions;
using BackPanel.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace BackPanel.Application.Interfaces;

public interface IRepositoryBase<TEntity> where TEntity : EntityBase
{
    Task<int> Complete(CancellationToken cancellationToken = default);
    Task<TEntity> CreateAsync(TEntity newItem, CancellationToken cancellationToken = default);
    Task CreateBulkAsync(List<TEntity> data, CancellationToken cancellationToken = default);
    void Delete<T>(T target) where T : EntityBase;
    Task DeleteAsync(int id, bool softDelete = true, CancellationToken cancellationToken = default);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity?> FirstOrDefaultAsync(params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity> GetById(int id, params Expression<Func<TEntity, object>>[] includes);
    Task<int> GetTotalRecords(Expression<Func<TEntity, bool>>? predicate = null);
    Task<List<TEntity>> ListAsync(List<Expression<Func<TEntity, bool>>>? predicates = null, params Expression<Func<TEntity, object>>[] includes);
    void PrepareDbSet(params Expression<Func<TEntity, object>>[] includes);
    IQueryable<TEntity> Query();
    Task<TEntity> UpdateAsync(TEntity newItem, CancellationToken cancellationToken = default);
}