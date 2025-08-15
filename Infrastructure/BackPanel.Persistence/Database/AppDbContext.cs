using BackPanel.Application.Helpers;
using BackPanel.Domain.Entities;
using BackPanel.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.Persistence.Database;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        builder.Entity<Admin>().HasIndex(c => c.Email).IsUnique();
        builder.Entity<Role>().HasIndex(c => c.Title).IsUnique();
    }

    
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<CompanyInfo> CompanyInfos => Set<CompanyInfo>();
}