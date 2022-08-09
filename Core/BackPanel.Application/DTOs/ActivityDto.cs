namespace BackPanel.Application.DTOs;

public class ActivityDto 
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string?  EffectedTable { get; set; }
    public int EffectedRowId { get; set; }
    public string? Action { get; set; }
    public DateTime CreatedAt { get; set; }
}