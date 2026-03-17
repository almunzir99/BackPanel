using BackPanel.Application.Helpers;
using BackPanel.Domain.Entities;
using BackPanel.Domain.Enums;
using BackPanel.Persistence.Identity;
using BackPanel.Shared.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace BackPanel.Persistence.Database;
public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        builder.Entity<Role>().HasIndex(c => c.Title).IsUnique();

        // AppUser -> Notification (one-to-many via Notification.UserId)
        builder.Entity<AppUser>()
            .HasMany(u => u.Notifications)
            .WithOne()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Activity -> AppUser (UserId FK)
        builder.Entity<Activity>()
            .HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

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
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<Message> Messages => Set<Message>();
    public new DbSet<Role> Roles => Set<Role>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<AppRole> AppRoles => Set<AppRole>();
}
