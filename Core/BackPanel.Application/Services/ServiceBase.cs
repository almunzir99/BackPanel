using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace BackPanel.Application.Services;

public abstract class ServiceBase<TEntity, TDto, TDtoRequest> : IServicesBase<TEntity, TDto, TDtoRequest>
    where TEntity : EntityBase where TDto : DtoBase
{
    protected readonly IRepositoryBase<TEntity> Repository;
    protected readonly IMapper Mapper;
    private readonly IRepositoryBase<Admin> _adminsRepository;

    protected ServiceBase(IMapper mapper, IRepositoryBase<TEntity> repository,
        IRepositoryBase<Admin> adminsRepository)
    {
        Repository = repository;
        Mapper = mapper;
        _adminsRepository = adminsRepository;
    }

    public virtual async Task CreateActivity(int userId, int rowId, string action)
    {
        var tableTitle = typeof(TEntity).Name;
        Activity activity = new Activity(userId, tableTitle, rowId, action, DateTime.Now);
        var user = await _adminsRepository.SingleAsync(userId);
        user.Activities.Add(activity);
        await _adminsRepository.Complete();
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

    public virtual async Task<String> ExportToCsv()
    {
        // list of properties
        Type type = typeof(TEntity);
        var title = type.Name;
        var properties = type.GetProperties().Where(c => c.PropertyType.IsPrimitive ||
                                                         c.PropertyType == typeof(String)
                                                         || c.PropertyType == typeof(DateTime)).ToList();
        var propertiesNames = properties.Select(c => c.Name);
        var data = new StringBuilder();

        string header = "";
        //create header 
        int index = 0;
        var enumerable = propertiesNames as string[] ?? propertiesNames.ToArray();
        foreach (var name in enumerable)
        {
            header += name;
            if (index < enumerable.Count() - 1)
                header += ",";
            index++;
        }

        data.AppendLine(header);
        var list = await Repository.ListAsync();
        var filteredProps = properties.Where(c => c.PropertyType.IsPrimitive ||
                                                  c.PropertyType == typeof(String)
                                                  || c.PropertyType == typeof(DateTime)).ToList();
        foreach (var item in list)
        {
            string value = "";
            int i = 0;
            foreach (var prop in filteredProps)
            {
                var propValue = prop.GetValue(item)?.ToString();
                propValue = propValue?.Replace(",", "");
                value += propValue;
                if (i < filteredProps.Count() - 1)
                    value += ",";
                i++;
            }

            data.AppendLine(value);
        }


        return data.ToString();
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
            .Where(c => (GetSearchPropValue(c) == string.Empty) || GetSearchPropValue(c).ToLower().Contains(search.ToLower()))
            .ToList();
        list = list
            .Skip((validFilter.PageIndex - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
        var result = Mapper.Map<IList<TEntity>, IList<TDto>>(list);
        return result;
    }

    protected List<TEntity> OrderBy(IList<TEntity> list, string prop, Boolean ascending)
    {
        //Get ordering Prop
        var type = typeof(TEntity);
        var orderProp = type.GetProperties().SingleOrDefault(c => c.Name.ToLower() == prop.ToLower());
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
        var searchProp = type.GetProperties().SingleOrDefault(c => c.Name.ToLower() == "name");
        var propValue = searchProp?.GetValue(obj)?.ToString();
        return propValue ?? String.Empty;
    }
}