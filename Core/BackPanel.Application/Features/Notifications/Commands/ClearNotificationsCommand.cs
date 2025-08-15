using MediatR;

namespace BackPanel.Application.Features.Notifications.Commands
{
    public record ClearNotificationsCommand(int UserId, string UserType) : IRequest;
}
