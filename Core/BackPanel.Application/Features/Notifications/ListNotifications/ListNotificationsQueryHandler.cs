using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Domain.Entities;
using MediatR;
using BackPanel.Application.Resolvers.UserResolver;

namespace BackPanel.Application.Features.Notifications.ListNotifications.ListNotifications
{

    public class ListNotificationsQueryHandler : IRequestHandler<ListNotificationsQuery, IList<NotificationDto>>
    {
        private readonly IUserResolver _userResolver;
        private readonly IMapper _mapper;

        public ListNotificationsQueryHandler(
            IUserResolver userResolver,
            IMapper mapper)
        {
            _userResolver = userResolver;
            _mapper = mapper;
        }

        public async Task<IList<NotificationDto>> Handle(ListNotificationsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userResolver.GetUserAsync(request.UserId, request.UserType);
            var notifications = user.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            return _mapper.Map<IList<Notification>, IList<NotificationDto>>(notifications);
        }
    }
}
