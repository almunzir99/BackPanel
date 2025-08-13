using BackPanel.Application.DTOs;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Features.Notifications.PushNotification
{
    public record PushNotificationCommand(
        int UserId,
        string UserType,
        NotificationDto Notification,
        UserEntityBase? Target = null) : IRequest;
}
