using System.Linq.Expressions;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOsRequests;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Interfaces;

public interface IAdminService : IUserBaseService<Admin, AdminDto, AdminDtoRequest>
{
    Task<IList<ActivityDto>> ActivitiesListAsync(PaginationFilter? filter);
     Task<IList<ActivityDto>> AdminActivitiesListAsync(int adminId, PaginationFilter? filter);
    Task<int> GetActivitiesTotalRecords(Expression<Func<Activity, bool>>? predicate = null);     

    
}