using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs;
using MediatR;

namespace BackPanel.Application.Features.Notifications.ListNotifications
{
    public record ListNotificationsQuery(
        int UserId,
        string UserType,
        PaginationFilter Filter) : IRequest<IList<NotificationDto>>;
}
