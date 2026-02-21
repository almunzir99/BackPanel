using BackPanel.Application.DTOs;

namespace BackPanel.Application.Interfaces;

public interface IIdentityRoleService
{
    Task<RoleDto?> GetByIdAsync(int id);
    Task<RoleDto?> GetByNameAsync(string roleName);
}