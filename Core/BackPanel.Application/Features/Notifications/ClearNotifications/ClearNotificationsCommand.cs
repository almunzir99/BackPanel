using MediatR;

namespace BackPanel.Application.Features.Notifications.ClearNotifications
{
    public record ClearNotificationsCommand(int UserId, string UserType) : IRequest;
}
