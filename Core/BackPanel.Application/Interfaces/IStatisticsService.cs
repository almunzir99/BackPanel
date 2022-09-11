namespace BackPanel.Application.Interfaces;

public interface IStatisticsService
{
    Task<StatisticsDto> getCounters();
}