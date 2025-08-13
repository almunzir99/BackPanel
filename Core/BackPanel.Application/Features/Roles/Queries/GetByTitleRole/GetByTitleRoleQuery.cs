using BackPanel.Application.DTOs;
using MediatR;

namespace BackPanel.Application.Features.Roles.Queries.GetByTitleRole 
{
    public record GetByTitleRoleQuery(string Title) : IRequest<RoleDto>;
}
