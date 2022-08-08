namespace BackPanel.Domain.Entities;
public abstract class EntityBase
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdate { get; set; }
    public int? CreatedBy { get; set; }
}