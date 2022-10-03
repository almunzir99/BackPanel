using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.FilesManager.Interfaces;
using BackPanel.SMTP.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.Application.Services;

public class AdminService : UserBaseService<Admin, AdminDto, AdminDtoRequest>, IAdminService
{
    private readonly IRepositoryBase<Activity> _activityRepository;
    public AdminService(IMapper mapper, ISmtpService smtpService, IFilesManagerService filesManagerService, IRepositoryBase<Admin> repository, IRepositoryBase<Admin> adminsRepository, IWebConfiguration webConfiguration, IRepositoryBase<Activity> activityRepository) : base(mapper, smtpService, filesManagerService, repository, adminsRepository, webConfiguration)
    {
        repository.IncludeableDbSet = repository.IncludeableDbSet
            .Include(c => c.Role);
        _activityRepository = activityRepository;
        _activityRepository.IncludeableDbSet = _activityRepository.IncludeableDbSet.Include(c => c.Admin); 
    }
    public async Task<IList<ActivityDto>> ActivitiesListAsync(PaginationFilter? filter)
    {
        var validFilter = (filter == null)
            ? new PaginationFilter()
            : new PaginationFilter(filter.PageIndex, filter.PageSize);
        var list = await _activityRepository.ListAsync();
        list = list.OrderByDescending(c => c.CreatedAt).ToList();
        list = list.Skip((validFilter.PageIndex - 1) * validFilter.PageSize)
         .Take(validFilter.PageSize).ToList();
        var result = Mapper.Map<IList<Activity>, IList<ActivityDto>>(list);
        return result;
    }
     public async Task<IList<ActivityDto>> AdminActivitiesListAsync(int adminId, PaginationFilter? filter)
    {
        var validFilter = (filter == null)
            ? new PaginationFilter()
            : new PaginationFilter(filter.PageIndex, filter.PageSize);
        var list = await _activityRepository.ListAsync();
        list = list.Where(c => c.AdminId == adminId).OrderByDescending(c => c.CreatedAt).ToList();
        list = list.Skip((validFilter.PageIndex - 1) * validFilter.PageSize)
         .Take(validFilter.PageSize).ToList();
        var result = Mapper.Map<IList<Activity>, IList<ActivityDto>>(list);
        return result;
    }
    public async Task<int> GetActivitiesCountAsync() => await _activityRepository.GetTotalRecords();


    protected override string UserType => "ADMIN";
}