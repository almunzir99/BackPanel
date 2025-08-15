using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Roles.Commands.Handlers;
public class UpdateRoleCommandHander : UpdateCommandHandlerBase<Role, RoleDtoRequest, RoleDto, UpdateCommandBase<RoleDtoRequest, RoleDto>>
{
    public UpdateRoleCommandHander(IRepositoryBase<Role> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}