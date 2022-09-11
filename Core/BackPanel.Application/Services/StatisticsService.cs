using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BackPanel.Application.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IRepositoryBase<Admin> _adminsRepository; 
    private readonly IRepositoryBase<Role> _rolesRepository; 
    private readonly IRepositoryBase<Message> _messagesRepository; 
    public StatisticsService(IServiceProvider injector)
    {
        _adminsRepository = injector.GetService<IRepositoryBase<Admin>>()!;
        _rolesRepository = injector.GetService<IRepositoryBase<Role>>()!;
        _messagesRepository = injector.GetService<IRepositoryBase<Message>>()!;

    }


    public async Task<StatisticsDto> getCounters()
    {
            var stats = new StatisticsDto();
            stats.admins = await _adminsRepository.GetTotalRecords();
            stats.messages = await _messagesRepository.GetTotalRecords();
            stats.roles = await _rolesRepository.GetTotalRecords();
            return stats;
    }
}