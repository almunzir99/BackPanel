using MediatR;

namespace BackPanel.Application.Features.Notifications.MarkNotificationsAsRead
{
    public record MarkNotificationsAsReadCommand(int UserId, string UserType) : IRequest;
}
