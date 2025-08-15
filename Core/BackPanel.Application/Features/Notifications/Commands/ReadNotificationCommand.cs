using BackPanel.Application.DTOs;
using MediatR;

namespace BackPanel.Application.Features.Notifications.Commands
{
    public record ReadNotificationCommand(int NotificationId) : IRequest<NotificationDto>;
}
