using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Features.Notifications.ReadNotification
{
    public class ReadNotificationCommandHandler : IRequestHandler<ReadNotificationCommand, NotificationDto>
    {
        private readonly IRepositoryBase<Notification> _repositoryBase;
        private readonly IMapper _mapper;

        public ReadNotificationCommandHandler(
            IRepositoryBase<Notification> repositoryBase,
            IMapper mapper)
        {
            _repositoryBase = repositoryBase;
            _mapper = mapper;
        }

        public async Task<NotificationDto> Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _repositoryBase.SingleAsync(request.NotificationId);
            if (notification == null)
                throw new Exception($"Notification with ID {request.NotificationId} not found");

            notification.Read = true;
            await _repositoryBase.Complete();
            return _mapper.Map<Notification, NotificationDto>(notification);
        }
    }
}
