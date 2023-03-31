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

    public async Task<StatisticsDto> GetCounters()
    {
        var stats = new StatisticsDto
        {
            Admins = await _adminsRepository.GetTotalRecords(),
            Messages = await _messagesRepository.GetTotalRecords(),
            Roles = await _rolesRepository.GetTotalRecords()
        };
        return stats;
    }
}