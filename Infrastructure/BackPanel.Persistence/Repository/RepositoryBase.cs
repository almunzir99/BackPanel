using System.Linq.Expressions;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.Persistence.Database;
using BackPanel.Persistence.Database.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.Persistence.Repository;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : EntityBase
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<TEntity> DbSet;
    private IQueryable<TEntity> _includeableDbSet;
    private readonly MapperHelper _mapperHelper;

    public IQueryable<TEntity> IncludeableDbSet
    {
        get => _includeableDbSet;
        set { _includeableDbSet = value ?? throw new ArgumentNullException(nameof(value), "value shouldn't be null"); }
    }

    public RepositoryBase(AppDbContext context, MapperHelper mapperHelper)
    {
        Context = context;
        _mapperHelper = mapperHelper;
        DbSet = Context.Set<TEntity>();
        _includeableDbSet = Context.Set<TEntity>();
    }

    public virtual async Task<TEntity> CreateAsync(TEntity item)
    {
        await DbSet.AddAsync(item);
        return item;
    }

    public virtual async Task DeleteAsync(int id)
    {
        var target = await _includeableDbSet.SingleOrDefaultAsync(c => c.Id == id);
        if (target == null)
            throw new Exception("The target Item doesn't Exist");
        DbSet.Remove(target);
    }

    public virtual void Delete<T>(T target) where  T: EntityBase
    {
        Context.Remove(target);
    }
    public virtual async Task<IList<TEntity>> ListAsync()
    {
        var list = await _includeableDbSet.ToListAsync();
        return list;
    }


    public virtual async Task<int> GetTotalRecords(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return(predicate != null) ? await DbSet.CountAsync(predicate) : await DbSet.CountAsync();
    }

    public async Task<IList<TEntity>> SearchAsync(Func<TEntity, bool> predicate)
    {
        var result = await _includeableDbSet.ToListAsync();
        result = result.Where(predicate).ToList();
        return result;
    }

    public virtual async Task<TEntity> SingleAsync(int id)
    {
        var result = await _includeableDbSet.SingleOrDefaultAsync(c => c.Id == id);
        if (result == null)
            throw new Exception("item is not found");
        return result;
    }

    public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var result = await _includeableDbSet.SingleOrDefaultAsync(predicate);
        if (result == null)
            throw new Exception("item is not found");
        return result;
    }

    public virtual async Task<TEntity> UpdateAsync(int id, TEntity newItem)
    {
        var result = await _includeableDbSet.SingleOrDefaultAsync(c => c.Id == id);
        if (result == null)
            throw new Exception("item is not found");
        Context.Attach(result);
        _mapperHelper.Map<TEntity, TEntity>(newItem, result,propsToExclude: new []{"Id","CreatedAt"}  );
        result.LastUpdate = DateTime.Now;
        return result;
    }

    public virtual async Task<TEntity> UpdateAsync(int id, JsonPatchDocument<TEntity> newItem)
    {
        var result = await _includeableDbSet.SingleOrDefaultAsync(c => c.Id == id);
        if (result == null)
            throw new Exception("item is not found");
        newItem.ApplyTo(result);
        result.LastUpdate = DateTime.Now;
        return result;
    }

    public async Task<int> Complete()
    {
        try
        {
            return await Context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            throw new Exception(exception.Decode());
        }
    }
}