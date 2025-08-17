using BackPanel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IRepositoryBase<Admin> AdminsRepository { get; }
        IRepositoryBase<CompanyInfo> CompanyInfosRepository { get; }
        IRepositoryBase<Message> MessagesRepository { get; }
        IRepositoryBase<Notification> NotificationsRepository { get; }
        IRepositoryBase<Permission> PermissionsRepository { get; }
        IRepositoryBase<Role> RolesRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
