using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Generic.Common.Queries;
using BackPanel.Application.Generic.Common.Queries.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Roles.Queries.Handlers;
public class GetAllRoleQueryHandler : GetAllQueryHandlerBase<Role, RoleDto, GetAllQueryBase<RoleDto>>
{
    public GetAllRoleQueryHandler(IRepositoryBase<Role> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}