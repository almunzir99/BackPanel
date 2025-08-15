using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Generic.Common.Queries;
using BackPanel.Application.Generic.Common.Queries.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Roles.Queries.Handlers;
public class ExportToExcelRoleQueryHandler : ExportToExcelQueryHandlerBase<Role, RoleDto, ExportToExcelQueryBase<Role>>
{
    public ExportToExcelRoleQueryHandler(IRepositoryBase<Role> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}