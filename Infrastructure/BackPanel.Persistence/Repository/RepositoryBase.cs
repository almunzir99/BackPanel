using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.Domain.Enums;
using BackPanel.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

 
public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
{
    private readonly AppDbContext _dbContext;
    private IQueryable<TEntity> _query;

    public RepositoryBase(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _query = _dbContext.Set<TEntity>();
    }

    // **CRUD Methods:**
    public async Task<TEntity> CreateAsync(TEntity newItem, CancellationToken cancellationToken = default)
    {
        try
        {
            if (newItem == null)
            {
                throw new ArgumentNullException(nameof(newItem), "New item cannot be null");
            }

            await _dbContext.Set<TEntity>().AddAsync(newItem, cancellationToken);
            return newItem;
        }
        catch (Exception ex) when (!(ex is ArgumentNullException))
        {
            throw new InvalidOperationException($"Failed to create entity of type {typeof(TEntity).Name}", ex);
        }
    }

    public async Task CreateBulkAsync(List<TEntity> data, CancellationToken cancellationToken = default)
    {
        try
        {
            if (data == null || !data.Any())
            {
                throw new ArgumentNullException(nameof(data), "Data cannot be null or empty");
            }

            // Validate all items are not null
            if (data.Any(item => item == null))
            {
                throw new ArgumentException("Data collection contains null items", nameof(data));
            }

            await _dbContext.Set<TEntity>().AddRangeAsync(data, cancellationToken);
        }
        catch (Exception ex) when (!(ex is ArgumentNullException || ex is ArgumentException))
        {
            throw new InvalidOperationException($"Failed to create bulk entities of type {typeof(TEntity).Name}", ex);
        }
    }

    public async Task<TEntity> UpdateAsync(TEntity newItem, CancellationToken cancellationToken = default)
    {
        try
        {
            if (newItem == null)
            {
                throw new ArgumentNullException(nameof(newItem), "New item cannot be null");
            }

            var existingItem = await _query.FirstOrDefaultAsync(x => x.Id == newItem.Id, cancellationToken);
            if (existingItem == null)
            {
                throw new KeyNotFoundException($"Entity with ID {newItem.Id} not found.");
            }

            existingItem.LastUpdate = DateTime.Now;
            _dbContext.Entry(existingItem).CurrentValues.SetValues(newItem);

            return existingItem;
        }
        catch (Exception ex) when (!(ex is ArgumentNullException || ex is KeyNotFoundException))
        {
            throw new InvalidOperationException($"Failed to update entity of type {typeof(TEntity).Name} with ID {newItem?.Id}", ex);
        }
    }

    public async Task DeleteAsync(int id, bool softDelete = true, CancellationToken cancellationToken = default)
    {
        try
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID must be greater than zero", nameof(id));
            }

            var entity = await _query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            if (softDelete)
            {
                entity.Status = Status.Deleted;
                _dbContext.Update(entity);
            }
            else
            {
                _dbContext.Set<TEntity>().Remove(entity);
            }
        }
        catch (Exception ex) when (!(ex is ArgumentException || ex is KeyNotFoundException))
        {
            throw new InvalidOperationException($"Failed to delete entity of type {typeof(TEntity).Name} with ID {id}", ex);
        }
    }

    public void Delete<T>(T target) where T : EntityBase
    {
        try
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target), "Target entity cannot be null");
            }

            _dbContext.Remove<T>(target);
        }
        catch (Exception ex) when (!(ex is ArgumentNullException))
        {
            throw new InvalidOperationException($"Failed to delete entity of type {typeof(T).Name}", ex);
        }
    }

    // **Read Methods:**
    public async Task<TEntity> GetById(int id, params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID must be greater than zero", nameof(id));
            }

            var query = _query.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    if (include != null)
                    {
                        query = query.Include(include);
                    }
                }
            }

            var entity = await query.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            return entity;
        }
        catch (Exception ex) when (!(ex is ArgumentException || ex is KeyNotFoundException))
        {
            throw new InvalidOperationException($"Failed to get entity of type {typeof(TEntity).Name} with ID {id}", ex);
        }
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "Predicate cannot be null");
            }

            var query = _query.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    if (include != null)
                    {
                        query = query.Include(include);
                    }
                }
            }

            return await query.FirstOrDefaultAsync(predicate);
        }
        catch (Exception ex) when (!(ex is ArgumentNullException))
        {
            throw new InvalidOperationException($"Failed to find entity of type {typeof(TEntity).Name}", ex);
        }
    }

    public async Task<TEntity?> FirstOrDefaultAsync(params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            var query = _query.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    if (include != null)
                    {
                        query = query.Include(include);
                    }
                }
            }

            return await query.FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to get first entity of type {typeof(TEntity).Name}", ex);
        }
    }

    // **Query Methods:**
    public Task<List<TEntity>> ListAsync(List<Expression<Func<TEntity, bool>>>? predicates = null, params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            if (predicates == null || !predicates.Any())
            {
                var simpleQuery = _query.AsQueryable();
                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        if (include != null)
                        {
                            simpleQuery = simpleQuery.Include(include);
                        }
                    }
                }
                return simpleQuery.ToListAsync();
            }

            IQueryable<TEntity> query = _query;
            foreach (var predicate in predicates)
            {
                if (predicate != null)
                {
                    query = query.Where(predicate);
                }
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    if (include != null)
                    {
                        query = query.Include(include);
                    }
                }
            }

            return query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to list entities of type {typeof(TEntity).Name}", ex);
        }
    }

    public IQueryable<TEntity> Query()
    {
        return _query.AsQueryable();
    }

    // **Utility Methods:**
    public async Task<int> GetTotalRecords(Expression<Func<TEntity, bool>>? predicate = null)
    {
        try
        {
            if (predicate == null)
            {
                return await _query.CountAsync();
            }
            return await _query.CountAsync(predicate);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to count entities of type {typeof(TEntity).Name}", ex);
        }
    }

    public Task<int> Complete(CancellationToken cancellationToken = default)
    {
        try
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InvalidOperationException("Concurrency conflict occurred while saving changes", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Database update failed", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to save changes to the database", ex);
        }
    }

    public void PrepareDbSet(params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    if (include != null)
                    {
                        _query = _query.Include(include);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to prepare DbSet with includes for type {typeof(TEntity).Name}", ex);
        }
    }
}