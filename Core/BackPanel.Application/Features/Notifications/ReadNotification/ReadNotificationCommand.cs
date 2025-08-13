using BackPanel.Application.DTOs;
using MediatR;

namespace BackPanel.Application.Features.Notifications.ReadNotification
{
    public record ReadNotificationCommand(int NotificationId) : IRequest<NotificationDto>;
}
