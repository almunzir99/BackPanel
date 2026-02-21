using BackPanel.Application.Features.Notifications.Commands;
using BackPanel.Application.Interfaces;
using MediatR;

namespace BackPanel.Application.Features.Notifications.Commands.Handlers
{
    public class BroadcastNotificationCommandHandler : IRequestHandler<BroadcastNotificationCommand>
    {
        private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public BroadcastNotificationCommandHandler(
            IUserService userService,
            IMediator mediator)
        {
            _userService = userService;
            _mediator = mediator;
        }

        public async Task Handle(BroadcastNotificationCommand request, CancellationToken cancellationToken)
        {
            var userIds = await _userService.GetAllUserIdsAsync();

            var pushTasks = userIds.Select(userId =>
                _mediator.Send(new PushNotificationCommand(userId, request.Notification), cancellationToken));

            await Task.WhenAll(pushTasks);
        }
    }
}
