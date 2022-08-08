namespace BackPanel.Domain.Entities;

public class Message : EntityBase
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Content { get; set; }
    public string? Phone { get; set; }
}