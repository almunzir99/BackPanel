using System.Linq.Expressions;
using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.Domain.Enums;
using BackPanel.FilesManager.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

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

    public virtual async Task<IList<TDto>> ListAsync(PaginationFilter? filter, IList<Func<TEntity, bool>>? conditions,
        string? search = "", string orderBy = "LastUpdate", bool ascending = true)
    {
        if (search == null) search = "";
        var validFilter = (filter == null)
            ? new PaginationFilter()
            : new PaginationFilter(filter.PageIndex, filter.PageSize);
        var list = await Repository.ListAsync();
        list = OrderBy(list, orderBy, ascending);
        if (conditions != default)
        {
            foreach (var condition in conditions)
            {
                list = list.Where(condition).ToList();
            }
        }

        list = list
            .Where(c => GetSearchPropValue(c)?.Length == 0 || GetSearchPropValue(c).Contains(search, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .Skip((validFilter.PageIndex - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
        var result = Mapper.Map<IList<TEntity>, IList<TDto>>(list);
        return result;
    }
    public async Task ActiveToggleAsync(int id)
    {
        var target = await Repository.SingleAsync(c => c.Id == id);
        if(target != null)
        {
             target.Status = target.Status == Status.Active ? Status.Disabled : Status.Active;
             await Repository.Complete();
        }
    }

    protected List<TEntity> OrderBy(IList<TEntity> list, string prop, Boolean ascending)
    {
        //Get ordering Prop
        var type = typeof(TEntity);
        var orderProp = type.GetProperties().SingleOrDefault(c => string.Equals(c.Name, prop, StringComparison.OrdinalIgnoreCase));
        if (orderProp == null)
            throw new Exception("ordering property isn't available");
        var orderedList = ascending
            ? list.OrderBy(c => orderProp.GetValue(c, null)).ToList()
            : list.OrderByDescending(c => orderProp.GetValue(c, null)).ToList();
        return orderedList;
    }

    public virtual async Task<TDto> SingleAsync(int id)
    {
        var result = await Repository.SingleAsync(id);
        var mappedResult = Mapper.Map<TEntity, TDto>(result);
        return mappedResult;
    }

    public virtual async Task<TDto> UpdateAsync(int id, TDtoRequest newItem)
    {
        var mappedItem = Mapper.Map<TDtoRequest, TEntity>(newItem);
        var result = await Repository.UpdateAsync(id, mappedItem);
        await Repository.Complete();
        var mappedResult = Mapper.Map<TEntity, TDto>(result);
        return mappedResult;
    }

    public virtual async Task<TDto> UpdateAsync(int id, JsonPatchDocument<TEntity> newItem)
    {
        var result = await Repository.UpdateAsync(id, newItem);
        await Repository.Complete();
        var mappedResult = Mapper.Map<TEntity, TDto>(result);
        return mappedResult;
    }

    protected virtual string GetSearchPropValue(TEntity obj)
    {
        var type = typeof(TEntity);
        var searchProp = type.GetProperties().SingleOrDefault(c => string.Equals(c.Name, "name", StringComparison.OrdinalIgnoreCase));
        var propValue = searchProp?.GetValue(obj)?.ToString();
        return propValue ?? String.Empty;
    }
}