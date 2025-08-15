using MediatR;

namespace BackPanel.Application.Features.Notifications.Commands
{
    public record MarkNotificationsAsReadCommand(int UserId, string UserType) : IRequest;
}
