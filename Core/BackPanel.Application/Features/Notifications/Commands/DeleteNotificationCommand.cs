using MediatR;

namespace BackPanel.Application.Features.Notifications.Commands
{
    public record DeleteNotificationCommand(int NotificationId) : IRequest;
}
