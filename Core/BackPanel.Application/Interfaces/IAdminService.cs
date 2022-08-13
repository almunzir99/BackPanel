using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Domain.Entities;

namespace BackPanel.Application.Interfaces;

public interface IAdminService : IUserBaseService<Admin, AdminDto, AdminDtoRequest>
{
    
}