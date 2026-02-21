using BackPanel.Application.DTOs.Filters;
using BackPanel.Application.DTOs;
using MediatR;

namespace BackPanel.Application.Features.Notifications.Queries
{
    public record ListNotificationsQuery(
        int UserId,
        PaginationFilter Filter) : IRequest<IList<NotificationDto>>;
}
