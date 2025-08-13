using MediatR;

namespace BackPanel.Application.Features.Notifications.DeleteNotification
{
    public record DeleteNotificationCommand(int NotificationId) : IRequest;
}
