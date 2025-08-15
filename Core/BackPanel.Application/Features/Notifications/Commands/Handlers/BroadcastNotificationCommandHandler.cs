using MediatR;
using BackPanel.Application.Resolvers.UserResolver;
using BackPanel.Application.Features.Notifications.Commands;

namespace BackPanel.Application.Features.Notifications.Commands.Handlers
{
    public class BroadcastNotificationCommandHandler : IRequestHandler<BroadcastNotificationCommand>
    {
        private readonly IUserResolver _userResolver;
        private readonly IMediator _mediator;

        public BroadcastNotificationCommandHandler(
            IUserResolver userResolver,
            IMediator mediator)
        {
            _userResolver = userResolver;
            _mediator = mediator;
        }

        public async Task Handle(BroadcastNotificationCommand request, CancellationToken cancellationToken)
        {
            var users = await _userResolver.GetUsersByTypeAsync(request.UserType);

            var pushTasks = users.Select(user =>
                _mediator.Send(new PushNotificationCommand(
                    user.Id,
                    request.UserType,
                    request.Notification,
                    user), cancellationToken));

            await Task.WhenAll(pushTasks);
        }
    }
}
