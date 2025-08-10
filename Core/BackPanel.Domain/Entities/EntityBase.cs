using BackPanel.Domain.Enums;

namespace BackPanel.Domain.Entities;
public abstract class EntityBase
{
    public int Id { get; set; }
    public Status Status { get; set; } = Status.Active;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastUpdate { get; set; } = DateTime.Now;
}