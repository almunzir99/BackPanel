using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Generic.Accounts.Commands;
using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using MediatR;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class CreateAdminCommandHander : CreateCommandHandlerBase<Admin, AdminDtoRequest, AdminDto, CreateCommandBase<AdminDtoRequest, AdminDto>>
    {
        private readonly IMediator mediator;
        public CreateAdminCommandHander(IRepositoryBase<Admin> repository, IMapper mapper, IMediator mediator) : base(repository, mapper)
        {
            this.mediator = mediator;
        }
        public override Task<AdminDto> Handle(CreateCommandBase<AdminDtoRequest, AdminDto> request, CancellationToken cancellationToken)
        {
            return mediator.Send(new RegisterCommandBase<AdminDtoRequest, AdminDto>(request.Request));
        }
    }
}
