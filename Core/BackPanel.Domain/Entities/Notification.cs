namespace BackPanel.Domain.Entities;

public class Notification : EntityBase
{
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? Action { get; set; }
    public string? Module { get; set; }
    public string? Url { get; set; }
    public bool Read { get; set; }
    public int? GroupedItem { get; set; }
}