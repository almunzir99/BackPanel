using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Generic.Common.Queries;
using BackPanel.Application.Generic.Common.Queries.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Roles.Queries.Handlers;
public class GetByIdRoleQueryHandler : GetByIdQueryHandlerBase<Role, RoleDto, GetByIdQueryBase<RoleDto>>
{
    public GetByIdRoleQueryHandler(IRepositoryBase<Role> repository, IMapper mapper) : base(repository, mapper)
    {
        repository.PrepareDbSet(x => x.AdminsPermissions!, x => x.RolesPermissions!, x => x.CompanyInfosPermissions!, x => x.MessagesPermissions!);

    }
}