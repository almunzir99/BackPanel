using AutoMapper;
using BackPanel.Application.DTOs;
using BackPanel.Application.DTOsRequests;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.FilesManager.Interfaces;
using BackPanel.SMTP.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.Application.Services;

public class AdminService : UserBaseService<Admin,AdminDto,AdminDtoRequest>, IAdminService
{
    public AdminService(IMapper mapper, ISmtpService smtpService, IFilesManagerService filesManagerService, IRepositoryBase<Admin> repository, IRepositoryBase<Admin> adminsRepository, IWebConfiguration webConfiguration) : base(mapper, smtpService, filesManagerService, repository, adminsRepository, webConfiguration)
    {
         repository.IncludeableDbSet = repository.IncludeableDbSet.Include(c => c.Activities)
            .Include(c => c.Role).Include(c => c.Role);
    }

    protected override string UserType => "ADMIN";
}