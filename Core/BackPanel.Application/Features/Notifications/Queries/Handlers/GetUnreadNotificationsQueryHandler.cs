using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Features.Notifications.Queries;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace BackPanel.Application.Features.Notifications.Queries.Handlers
{
    public class GetUnreadNotificationsQueryHandler : IRequestHandler<GetUnreadNotificationsQuery, IList<NotificationDto>>
    {
        private readonly IRepositoryBase<Notification> _repositoryBase;
        private readonly IMapper _mapper;

        public GetUnreadNotificationsQueryHandler(
            IRepositoryBase<Notification> repositoryBase,
            IMapper mapper)
        {
            _repositoryBase = repositoryBase;
            _mapper = mapper;
        }

        public async Task<IList<NotificationDto>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
        {
            var predicates = new List<Expression<Func<Notification, bool>>>
            {
                n => n.UserId == request.UserId,
                n => !n.Read
            };
            var notifications = await _repositoryBase.ListAsync(predicates);
            return _mapper.Map<IList<Notification>, IList<NotificationDto>>(notifications);
        }
    }
}