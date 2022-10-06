using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.Application.Services;

public class RolesService : ServiceBase<Role,RoleDto,RoleDtoRequest>, IRolesService
{
    public RolesService(IMapper mapper, IRepositoryBase<Role> repository, IRepositoryBase<Admin> adminsRepository) : base(mapper, repository, adminsRepository)
    {
        repository.IncludeableDbSet = repository.IncludeableDbSet.Include(c => c.MessagesPermissions)
            .Include(c => c.AdminsPermissions)
            .Include(c => c.RolesPermissions);
    }

    public async Task<Role> GetRoleByTitle(string title)
    {
        var result =  await Repository.SingleAsync(c => c.Title != null && c.Title.Equals(title));
        if(result != null)
        return result;
        else throw new Exception("The target role not found");
    }
}