using BackPanel.Application.Features.Notifications.Commands;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Features.Notifications.Commands.Handlers
{
    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand>
    {
        private readonly IRepositoryBase<Notification> _repositoryBase;

        public DeleteNotificationCommandHandler(IRepositoryBase<Notification> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            await _repositoryBase.DeleteAsync(request.NotificationId);
            await _repositoryBase.Complete();
        }
    }
}
