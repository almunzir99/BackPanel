using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IRepositoryBase<Notification> _repositoryBase;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Admin> _adminRepository;

    public NotificationService(IRepositoryBase<Notification> repositoryBase, IMapper mapper,
        IRepositoryBase<Admin> adminRepository)
    {
        _repositoryBase = repositoryBase;
        _mapper = mapper;
        _adminRepository = adminRepository;
        _adminRepository.IncludeableDbSet.Include(c => c.Notifications);
    }

    public async Task ClearNotificationAsync(int userId, string userType)
    {
        var user = await GetUser(userId, userType);
        user.Notifications.Clear();
        await _repositoryBase.Complete();
    }

    public async Task DeleteNotificationAsync(int id)
    {
        await _repositoryBase.DeleteAsync(id);
        await _repositoryBase.Complete();
    }

    public async Task<IList<NotificationDto>> GetUnreadNotification(int userId, string userType, bool autoRead)
    {
        var user = await GetUser(userId, userType);
        var notifications = user.Notifications.Where(c => c.Read == false).ToList();
        if (autoRead == true)
        {
            foreach (var notification in notifications)
            {
                notification.Read = true;
            }

            await _repositoryBase.Complete();
        }

        var mappedNotifications = _mapper.Map<IList<Notification>, IList<NotificationDto>>(notifications);
        return mappedNotifications;
    }

    public async Task<IList<NotificationDto>> ListNotificationsAsync(int userId, string userType,
        PaginationFilter filter)
    {
        var user = await GetUser(userId, userType);
        var notifications = user.Notifications.OrderByDescending(c => c.CreatedAt).ToList();
        var mappedNotifications = _mapper.Map<IList<Notification>, IList<NotificationDto>>(notifications);
        return mappedNotifications;
    }

    public async Task PushNotification(int userId, string userType, NotificationDto notification)
    {
        UserEntityBase user = await GetUser(userId, userType);
        var mappedNotification = _mapper.Map<NotificationDto, Notification>(notification);
        user.Notifications.Add(mappedNotification);
        var result = _mapper.Map<Notification, NotificationDto>(mappedNotification);
        await _repositoryBase.Complete();
        // await PushNotificationWithSignalR(userId, userType, result);
    }

    private async Task<UserEntityBase> GetUser(int userId, string userType)
    {
        UserEntityBase user;
        if (userType.ToLower() == "admin")
            user = await _adminRepository.SingleAsync(userId);
        else
            throw new Exception("userType should either student, instructor or admin");
        if (user == null)
            throw new Exception("target user isn't available");
        return user;
    }

    public async Task<NotificationDto> ReadNotification(int id)
    {
        var notification = await _repositoryBase.SingleAsync(id);
        if (notification == null)
            throw new Exception("target notification isn't available");
        notification.Read = true;
        await _repositoryBase.Complete();
        var mappedNotification = _mapper.Map<Notification, NotificationDto>(notification);
        return mappedNotification;
    }


    public async Task BroadCastNotification(NotificationDto notification, string userType,
        IList<Func<UserEntityBase>>? conditions = null)
    {
        IList<UserEntityBase> users;
        if (userType.ToLower() == "admin")
        {
            var admins = await _adminRepository.ListAsync();
            users = admins.Cast<UserEntityBase>().ToList();
        }
        else
            throw new Exception("userType should either customer, delivery or admin");

        var mappedNotification = _mapper.Map<NotificationDto, Notification>(notification);

        foreach (var user in users)
        {
            user.Notifications.Add(mappedNotification);
        }

        await _repositoryBase.Complete();
        foreach (var user in users)
        {
            // await PushNotificationWithSignalR(user.Id, userType, notification);
        }
    }

    // private async Task PushNotificationWithSignalR(int userId, string userType, NotificationDto notification)
    // {
    //     var target = _connectionManager.GetUserConnections(userId, userType);
    //     foreach (var id in target)
    //     {
    //         await _hubContext.Clients.Client(id).SendAsync("recieveNotification", notification);
    //     }
    // }
}