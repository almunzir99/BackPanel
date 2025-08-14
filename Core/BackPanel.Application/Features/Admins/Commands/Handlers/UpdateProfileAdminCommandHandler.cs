using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Generic.Accounts.Commands;
using BackPanel.Application.Generic.Accounts.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class UpdateProfileAdminCommandHandler : UpdateProfileCommandHandlerBase<Admin, AdminDtoRequest, AdminDto, UpdateProfileCommandBase<AdminDtoRequest, AdminDto>>
    {
        public UpdateProfileAdminCommandHandler(IRepositoryBase<Admin> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
