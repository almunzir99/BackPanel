using BackPanel.Application.DTOs;
using MediatR;

namespace BackPanel.Application.Features.Notifications.Commands
{
    public record BroadcastNotificationCommand(NotificationDto Notification) : IRequest;
}
