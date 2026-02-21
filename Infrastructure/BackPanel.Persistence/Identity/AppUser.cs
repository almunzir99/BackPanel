using BackPanel.Domain.Entities;
using BackPanel.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace BackPanel.Persistence.Identity;

public class AppUser : IdentityUser<int>
{
    public bool IsManager { get; set; }
    public string? Image { get; set; }
    public Status Status { get; set; } = Status.Active;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastUpdate { get; set; } = DateTime.Now;
    public IList<Notification> Notifications { get; set; } = new List<Notification>();
}
