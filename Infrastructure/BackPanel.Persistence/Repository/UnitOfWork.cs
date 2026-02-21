using BackPanel.Application.Helpers;
using BackPanel.Application.Interfaces;
using BackPanel.Domain.Entities;
using BackPanel.Persistence.Database;

namespace BackPanel.Persistence.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public IRepositoryBase<Message> MessagesRepository { get; }
    public IRepositoryBase<Role> RolesRepository { get; }
    public IRepositoryBase<Notification> NotificationsRepository { get; }
    public IRepositoryBase<CompanyInfo> CompanyInfosRepository { get; }

    public UnitOfWork(AppDbContext dbContext, MapperHelper mapperHelper)
    {
        MessagesRepository = new RepositoryBase<Message>(dbContext);
        RolesRepository = new RepositoryBase<Role>(dbContext);
        NotificationsRepository = new RepositoryBase<Notification>(dbContext);
        CompanyInfosRepository = new RepositoryBase<CompanyInfo>(dbContext);
        this._dbContext = dbContext;
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}