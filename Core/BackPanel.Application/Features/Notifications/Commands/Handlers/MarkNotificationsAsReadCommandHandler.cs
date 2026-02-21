using BackPanel.Application.Features.Notifications.Commands;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace BackPanel.Application.Features.Notifications.Commands.Handlers
{
    public class MarkNotificationsAsReadCommandHandler : IRequestHandler<MarkNotificationsAsReadCommand>
    {
        private readonly IRepositoryBase<Notification> _repositoryBase;

        public MarkNotificationsAsReadCommandHandler(IRepositoryBase<Notification> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public async Task Handle(MarkNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var predicates = new List<Expression<Func<Notification, bool>>>
            {
                n => n.UserId == request.UserId,
                n => !n.Read
            };
            var unread = await _repositoryBase.ListAsync(predicates);
            foreach (var n in unread)
                n.Read = true;
            await _repositoryBase.Complete();
        }
    }
}