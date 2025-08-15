using AutoMapper;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Generic.Common.Commands;
using BackPanel.Application.Generic.Common.Commands.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Roles.Commands.Handlers;
public class CreateBulkRoleCommandHander : CreateBulkCommandHandlerBase<Role, RoleDtoRequest, CreateBulkCommandBase<RoleDtoRequest>>
{
    public CreateBulkRoleCommandHander(IRepositoryBase<Role> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}