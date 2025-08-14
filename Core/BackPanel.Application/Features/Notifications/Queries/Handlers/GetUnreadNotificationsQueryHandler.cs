using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Domain.Entities;
using MediatR;
using BackPanel.Application.Resolvers.UserResolver;
using BackPanel.Application.Features.Notifications.Queries;

namespace BackPanel.Application.Features.Notifications.Queries.Handlers
{
    public class GetUnreadNotificationsQueryHandler : IRequestHandler<GetUnreadNotificationsQuery, IList<NotificationDto>>
    {
        private readonly IUserResolver _userResolver;
        private readonly IMapper _mapper;

        public GetUnreadNotificationsQueryHandler(
            IUserResolver userResolver,
            IMapper mapper)
        {
            _userResolver = userResolver;
            _mapper = mapper;
        }

        public async Task<IList<NotificationDto>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userResolver.GetUserAsync(request.UserId, request.UserType);
            var unreadNotifications = user.Notifications.Where(n => !n.Read).ToList();
            return _mapper.Map<IList<Notification>, IList<NotificationDto>>(unreadNotifications);
        }
    }
}
