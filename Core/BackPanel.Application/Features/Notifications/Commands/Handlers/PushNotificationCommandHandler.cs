using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Features.Notifications.Commands;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Features.Notifications.Commands.Handlers
{
    public class PushNotificationCommandHandler : IRequestHandler<PushNotificationCommand>
    {
        private readonly IRepositoryBase<Notification> _repositoryBase;
        private readonly IMapper _mapper;

        public PushNotificationCommandHandler(
            IRepositoryBase<Notification> repositoryBase,
            IMapper mapper)
        {
            _repositoryBase = repositoryBase;
            _mapper = mapper;
        }

        public async Task Handle(PushNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = _mapper.Map<NotificationDto, Notification>(request.Notification);
            notification.UserId = request.UserId;
            await _repositoryBase.CreateAsync(notification);
            await _repositoryBase.Complete();
        }
    }
}