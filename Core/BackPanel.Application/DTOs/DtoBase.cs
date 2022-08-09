namespace BackPanel.Application.DTOs;

public abstract class DtoBase
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdate { get; set; }
}