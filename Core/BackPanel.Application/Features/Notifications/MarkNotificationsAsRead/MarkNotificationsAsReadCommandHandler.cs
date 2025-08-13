using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;
using BackPanel.Application.Resolvers.UserResolver;

namespace BackPanel.Application.Features.Notifications.MarkNotificationsAsRead
{
    public class MarkNotificationsAsReadCommandHandler : IRequestHandler<MarkNotificationsAsReadCommand>
    {
        private readonly IRepositoryBase<Notification> _repositoryBase;
        private readonly IUserResolver _userResolver;

        public MarkNotificationsAsReadCommandHandler(
            IRepositoryBase<Notification> repositoryBase,
            IUserResolver userResolver)
        {
            _repositoryBase = repositoryBase;
            _userResolver = userResolver;
        }

        public async Task Handle(MarkNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var user = await _userResolver.GetUserAsync(request.UserId, request.UserType);
            var unreadNotifications = user.Notifications.Where(n => !n.Read);

            foreach (var notification in unreadNotifications)
            {
                notification.Read = true;
            }

            await _repositoryBase.Complete();
        }
    }
}
