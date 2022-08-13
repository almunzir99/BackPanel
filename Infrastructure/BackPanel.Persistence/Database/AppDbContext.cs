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
        builder.Entity<Admin>().HasData(GetManagerUser());
        builder.Entity<Role>().HasIndex(c => c.Title).IsUnique();

    }
    private Admin GetManagerUser()
    {
        HashingHelper.CreateHashPassword("maze@0099", out var pHash, out var pSalt);
        Admin admin = new Admin()
        {
            Id = 1,
            Username = "almunzir99",
            PasswordHash = pHash,
            PasswordSalt = pSalt,
            Phone = "249128647019",
            Email = "almunzir99@gmail.com",
            IsManager = true,
            CreatedAt = System.DateTime.Now,
            LastUpdate = System.DateTime.Now


        };
        return admin;

    }
    public DbSet<Admin> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Notification> Notifications { get; set; }

}