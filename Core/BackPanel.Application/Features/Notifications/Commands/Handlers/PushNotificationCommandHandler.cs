using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;
using BackPanel.Application.Resolvers.UserResolver;
using BackPanel.Application.Features.Notifications.Commands;

namespace BackPanel.Application.Features.Notifications.Commands.Handlers
{
    public class PushNotificationCommandHandler : IRequestHandler<PushNotificationCommand>
    {
        private readonly IRepositoryBase<Notification> _repositoryBase;
        private readonly IMapper _mapper;
        private readonly IUserResolver _userResolver;

        public PushNotificationCommandHandler(
            IRepositoryBase<Notification> repositoryBase,
            IMapper mapper,
            IUserResolver userResolver)
        {
            _repositoryBase = repositoryBase;
            _mapper = mapper;
            _userResolver = userResolver;
        }

        public async Task Handle(PushNotificationCommand request, CancellationToken cancellationToken)
        {
            var user = request.Target ?? await _userResolver.GetUserAsync(request.UserId, request.UserType);
            var mappedNotification = _mapper.Map<NotificationDto, Notification>(request.Notification);
            user.Notifications.Add(mappedNotification);
            await _repositoryBase.Complete();
        }
    }
}
