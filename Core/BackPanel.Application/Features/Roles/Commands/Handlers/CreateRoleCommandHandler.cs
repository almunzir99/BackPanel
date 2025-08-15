using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Roles.Commands.Handlers;
public class CreateRoleCommandHander : CreateCommandHandlerBase<Role, RoleDtoRequest, RoleDto, CreateCommandBase<RoleDtoRequest, RoleDto>>
{
    public CreateRoleCommandHander(IRepositoryBase<Role> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}