using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Generic.Accounts.Commands;
using BackPanel.Application.Generic.Accounts.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class RegisterAdminCommandHandler : RegisterCommandHandlerBase<Admin, AdminDtoRequest, AdminDto, RegisterCommandBase<AdminDtoRequest, AdminDto>>
    {
        public RegisterAdminCommandHandler(IRepositoryBase<Admin> repository, IMapper mapper, IWebConfiguration webConfiguration) : base(repository, mapper, webConfiguration)
        {
        }

        protected override string UserType => "ADMIN";
    }
}
