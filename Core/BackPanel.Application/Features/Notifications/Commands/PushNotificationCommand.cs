using BackPanel.Application.DTOs;
using MediatR;

namespace BackPanel.Application.Features.Notifications.Commands
{
    public record PushNotificationCommand(
        int UserId,
        NotificationDto Notification) : IRequest;
}
