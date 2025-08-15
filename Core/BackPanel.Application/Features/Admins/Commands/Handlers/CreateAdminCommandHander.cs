using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Admins.Commands.Handlers
{
    public class CreateAdminCommandHander : CreateCommandHandlerBase<Admin, AdminDtoRequest, AdminDto, CreateCommandBase<AdminDtoRequest, AdminDto>>
    {
        public CreateAdminCommandHander(IRepositoryBase<Admin> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
