using BackPanel.Application.DTOs;
using MediatR;

namespace BackPanel.Application.Features.Notifications.Queries
{
    public record GetUnreadNotificationsQuery(int UserId, string UserType) : IRequest<IList<NotificationDto>>;
}
