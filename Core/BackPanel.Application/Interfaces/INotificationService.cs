using BackPanel.Application.DTOs;
using BackPanel.Application.DTOs.Filters;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Interfaces;

public interface INotificationService
{
    Task PushNotification(int userId, string userType, NotificationDto notification,UserEntityBase? target = null);
    Task<NotificationDto> ReadNotification(int id);
    Task BroadCastNotification(NotificationDto notification, string userType, IList<Func<UserEntityBase>>? conditions = null);
    Task<IList<NotificationDto>> GetUnreadNotification(int userId,string userType,bool autoRead);
    Task<IList<NotificationDto>> ListNotificationsAsync(int userId, string userType, PaginationFilter filter);
    Task DeleteNotificationAsync(int id);
    Task ClearNotificationAsync(int userId,string userType);
}