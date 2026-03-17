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
        IRepositoryBase<Business> BusinessesRepository { get; }
        IRepositoryBase<Message> MessagesRepository { get; }
        IRepositoryBase<Notification> NotificationsRepository { get; }
        IRepositoryBase<Role> RolesRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
