using System.Linq.Expressions;
using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Extensions;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.Domain.Enums;
using BackPanel.FilesManager.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.Application.Services;

public abstract class ServiceBase<TEntity, TDto, TDtoRequest> : IServicesBase<TEntity, TDto, TDtoRequest>
    where TEntity : EntityBase where TDto : DtoBase
{
    protected readonly IRepositoryBase<TEntity> Repository;
    protected readonly IMapper Mapper;
    private readonly IRepositoryBase<Admin> _adminsRepository;
    protected readonly IPathProvider PathProvider;
    protected ServiceBase(IMapper mapper, IRepositoryBase<TEntity> repository,
        IRepositoryBase<Admin> adminsRepository, IPathProvider pathProvider)
    {
        Repository = repository;
        Mapper = mapper;
        _adminsRepository = adminsRepository;
        PathProvider = pathProvider;
    }

    public virtual async Task CreateActivity(int userId, int rowId, string action)
    {
        var tableTitle = typeof(TEntity).Name;
        Activity activity = new(userId, tableTitle, rowId, action, DateTime.Now);
        var user = await _adminsRepository.SingleAsync(userId);
        user.Activities.Add(activity);
        await _adminsRepository.Complete();
    }
    public virtual async Task CreateAllAsync(IList<TDtoRequest> newItems)
    {
        var mappedList = Mapper.Map<IList<TDtoRequest>, IList<TEntity>>(newItems);
        foreach (var item in mappedList)
        {
            item.CreatedAt = DateTime.Now;
            item.LastUpdate = DateTime.Now;
            await Repository.CreateAsync(item);
        }
        await Repository.Complete();
    }
    public virtual async Task<TDto> CreateAsync(TDtoRequest newItem)
    {
        var mappedItem = Mapper.Map<TDtoRequest, TEntity>(newItem);
        mappedItem.CreatedAt = DateTime.Now;
        mappedItem.LastUpdate = DateTime.Now;
        var savedItem = await Repository.CreateAsync(mappedItem);
        await Repository.Complete();
        var result = Mapper.Map<TEntity, TDto>(savedItem);
        return result;
    }

    public virtual async Task DeleteAsync(int id)
    {
        await Repository.DeleteAsync(id);
        await Repository.Complete();
    }

    public virtual async Task<Byte[]> ExportToExcel()
    {
        var data = await Repository.ListAsync();
        return DataExportHelper<TEntity>.ExportToExcel(data);
    }
    public virtual async Task<Byte[]> ExportToPdf()
    {
        var data = await Repository.ListAsync();
        return await DataExportHelper<TEntity>.ExportToPdfAsync(data, PathProvider.GetBaseUrl());
    }

    public virtual async Task<int> GetTotalRecords(Expression<Func<TEntity, bool>>? predicate = null) => await Repository.GetTotalRecords(predicate);

    public virtual async Task<(IList<TDto>,int)> ListAsync(PaginationFilter? filter,
        string? search = "", string orderBy = "LastUpdate", bool ascending = true, IList<SearchExpressionDtoRequest>? expressions = null)
    {
        if (search == null) search = "";
        var validFilter = (filter == null)
            ? new PaginationFilter()
            : new PaginationFilter(filter.PageIndex, filter.PageSize);
        var list = Repository.List();
        var query = list.Select(c => Mapper.Map<TDto>(c));
        var result = await query.ToListAsync();
        var total = result.Count;
        // // Apply Order
        result = result.OrderByProperty(orderBy,ascending).ToList();
        // Apply search Expressions
        if (expressions != null)
        {
            foreach (var expression in expressions)
            {
                var lambda = ExpressionBuilder.BuildComparisonExpression<TDto>(expression.PropName!, expression.Operator, expression.PropValue!);
                result = result.Where(lambda.Compile()).ToList();
            }
        }
         // apply unified search Expressions
        var expr = GetUnifiedSearchExpression(search);
        var func = expr?.Compile();
        result = result.Where(func ?? (c => false)).ToList();
        // Apply Pagination
        result = result
            .Skip((validFilter.PageIndex - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
        return (result,total);
    }
    public async Task ActiveToggleAsync(int id)
    {
        var target = await Repository.SingleAsync(c => c.Id == id);
        if (target != null)
        {
            target.Status = target.Status == Status.Active ? Status.Disabled : Status.Active;
            await Repository.Complete();
        }
    }

    protected IQueryable<TEntity> OrderBy(IQueryable<TEntity> list, string prop, Boolean ascending)
    {
        //Get ordering Prop
        var type = typeof(TEntity);
        var orderProp = type.GetProperties().SingleOrDefault(c => string.Equals(c.Name, prop, StringComparison.OrdinalIgnoreCase));
        if (orderProp == null)
            throw new Exception("ordering property isn't available");
        return ascending
            ? list.OrderBy(c => orderProp.GetValue(c, null))
            : list.OrderByDescending(c => orderProp.GetValue(c, null));
    }

    public virtual async Task<TDto> SingleAsync(int id)
    {
        var result = await Repository.SingleAsync(id);
        return Mapper.Map<TEntity, TDto>(result);
    }

    public virtual async Task<TDto> UpdateAsync(int id, TDtoRequest newItem)
    {
        var mappedItem = Mapper.Map<TDtoRequest, TEntity>(newItem);
        var result = await Repository.UpdateAsync(id, mappedItem);
        await Repository.Complete();
        return Mapper.Map<TEntity, TDto>(result);
    }

    public virtual async Task<TDto> UpdateAsync(int id, JsonPatchDocument<TEntity> newItem)
    {
        var result = await Repository.UpdateAsync(id, newItem);
        await Repository.Complete();
        return Mapper.Map<TEntity, TDto>(result);
    }

    protected virtual string GetSearchPropValue(TEntity obj)
    {
        var type = typeof(TEntity);
        var searchProp = type.GetProperties().SingleOrDefault(c => string.Equals(c.Name, "name", StringComparison.OrdinalIgnoreCase));
        var propValue = searchProp?.GetValue(obj)?.ToString();
        return propValue ?? String.Empty;
    }
     private static Expression<Func<TDto,bool>>? GetUnifiedSearchExpression(string? search)
    {
        var parameter = Expression.Parameter(typeof(TDto), "e");
        var predicates = typeof(TDto).GetProperties().Where(c => c.PropertyType.IsPrimitive || c.PropertyType == typeof(string) 
        || c.PropertyType == typeof(DateTime)).Select(propertyInfo =>
        {
            Expression propertyAccess = Expression.Property(parameter, propertyInfo);
            // Convert non-string properties to string before calling Contains
            if (propertyInfo.PropertyType != typeof(string))
            {
                propertyAccess = Expression.Call(propertyAccess, typeof(object).GetMethod("ToString")!);
            }
            propertyAccess =  Expression.Condition(Expression.Equal(propertyAccess, Expression.Constant(default)),
                Expression.Constant("", typeof(string)),propertyAccess);
            var searchTermExpression = Expression.Constant(search, typeof(string));
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            Expression containsCall = Expression.Call(propertyAccess, containsMethod!, searchTermExpression);
            return containsCall;

        }).ToList();
        var orExpression = predicates
            .Aggregate((accumulate, predicate) => Expression.Or(accumulate, predicate));
        var finalExpression = Expression.Lambda<Func<TDto, bool>>(orExpression, parameter);
        return finalExpression;
    }
    
}