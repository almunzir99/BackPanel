using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;
using BackPanel.Application.Resolvers.UserResolver;
using BackPanel.Application.Features.Notifications.Commands;

namespace BackPanel.Application.Features.Notifications.Commands.Handlers
{
    public class ClearNotificationsCommandHandler : IRequestHandler<ClearNotificationsCommand>
    {
        private readonly IRepositoryBase<Notification> _repositoryBase;
        private readonly IUserResolver _userResolver;

        public ClearNotificationsCommandHandler(
            IRepositoryBase<Notification> repositoryBase,
            IUserResolver userResolver)
        {
            _repositoryBase = repositoryBase;
            _userResolver = userResolver;
        }

        public async Task Handle(ClearNotificationsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userResolver.GetUserAsync(request.UserId, request.UserType);
            user.Notifications.Clear();
            await _repositoryBase.Complete();
        }
    }
}
