using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.Generic.Common.Queries;
using BackPanel.Application.Generic.Common.Queries.Handlers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Features.Roles.Queries.Handlers
{
    public class ExportToPDFRoleQueryHandler : ExportToPDFQueryHandlerBase<Role, RoleDto, ExportToPDFQueryBase<Role>>
    {
        public ExportToPDFRoleQueryHandler(IRepositoryBase<Role> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}