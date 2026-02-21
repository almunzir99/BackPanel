using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Features.Notifications.Queries;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace BackPanel.Application.Features.Notifications.Queries.Handlers
{
    public class ListNotificationsQueryHandler : IRequestHandler<ListNotificationsQuery, IList<NotificationDto>>
    {
        private readonly IRepositoryBase<Notification> _repositoryBase;
        private readonly IMapper _mapper;

        public ListNotificationsQueryHandler(
            IRepositoryBase<Notification> repositoryBase,
            IMapper mapper)
        {
            _repositoryBase = repositoryBase;
            _mapper = mapper;
        }

        public async Task<IList<NotificationDto>> Handle(ListNotificationsQuery request, CancellationToken cancellationToken)
        {
            var predicates = new List<Expression<Func<Notification, bool>>>
            {
                n => n.UserId == request.UserId
            };
            var notifications = await _repositoryBase.ListAsync(predicates);
            var sorted = notifications.OrderByDescending(n => n.CreatedAt).ToList();
            return _mapper.Map<IList<Notification>, IList<NotificationDto>>(sorted);
        }
    }
}