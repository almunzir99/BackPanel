using BackPanel.Application.Helpers;
using BackPanel.Domain.Entities;
using BackPanel.Domain.Enums;
using BackPanel.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Emit;

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

        ConfigureDeleteStatusFilter(builder);

    }

    private static void ConfigureDeleteStatusFilter(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(EntityBase).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var statusProperty = Expression.Property(parameter, nameof(EntityBase.Status));
                var deletedStatus = Expression.Constant(Status.Deleted);
                var notEqual = Expression.NotEqual(statusProperty, deletedStatus);
                var lambda = Expression.Lambda(notEqual, parameter);

                builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }
    public IQueryable<T> WithDeleted<T>() where T : EntityBase
    {
        return Set<T>().IgnoreQueryFilters();
    }
    public IQueryable<T> OnlyDeleted<T>() where T : EntityBase
    {
        return Set<T>().IgnoreQueryFilters().Where(e => e.Status == Status.Deleted);
    }
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<CompanyInfo> CompanyInfos => Set<CompanyInfo>();
}