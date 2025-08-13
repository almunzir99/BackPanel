using BackPanel.Application.DTOs;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Features.Notifications.BroadcastNotification
{
    public record BroadcastNotificationCommand(
        NotificationDto Notification,
        string UserType,
        IList<Func<UserEntityBase>>? Conditions = null) : IRequest;
}
