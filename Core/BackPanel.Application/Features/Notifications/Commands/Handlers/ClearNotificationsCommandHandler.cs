using BackPanel.Application.Features.Notifications.Commands;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace BackPanel.Application.Features.Notifications.Commands.Handlers
{
    public class ClearNotificationsCommandHandler : IRequestHandler<ClearNotificationsCommand>
    {
        private readonly IRepositoryBase<Notification> _repositoryBase;

        public ClearNotificationsCommandHandler(IRepositoryBase<Notification> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public async Task Handle(ClearNotificationsCommand request, CancellationToken cancellationToken)
        {
            var predicates = new List<Expression<Func<Notification, bool>>>
            {
                n => n.UserId == request.UserId
            };
            var notifications = await _repositoryBase.ListAsync(predicates);
            foreach (var n in notifications)
                await _repositoryBase.DeleteAsync(n.Id);
            await _repositoryBase.Complete();
        }
    }
}