using BackPanel.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace BackPanel.Persistence.Identity;

public class AppRole : IdentityRole<int>
{
    public Status Status { get; set; } = Status.Active;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastUpdate { get; set; } = DateTime.Now;
    public int? LegacyRoleId { get; set; }
}
