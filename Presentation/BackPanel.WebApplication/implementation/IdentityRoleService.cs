using BackPanel.Application.DTOs;
using BackPanel.Application.Interfaces;
using BackPanel.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.WebApplication.implementation;

public class IdentityRoleService : IIdentityRoleService
{
    private readonly RoleManager<AppRole> _roleManager;

    public IdentityRoleService(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<RoleDto?> GetByIdAsync(int id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        return Map(role);
    }

    public async Task<RoleDto?> GetByNameAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return null;

        var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Name == roleName);
        return Map(role);
    }

    private static RoleDto? Map(AppRole? role)
    {
        if (role == null)
            return null;

        return new RoleDto
        {
            Id = role.Id,
            Title = role.Name,
            Status = role.Status,
            CreatedAt = role.CreatedAt,
            LastUpdate = role.LastUpdate,
        };
    }
}