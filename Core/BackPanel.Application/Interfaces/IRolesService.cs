using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Interfaces;

public interface IRolesService : IServicesBase<Role,RoleDto,RoleDtoRequest>
{
    Task<Role> GetRoleByTitle(string title);
}