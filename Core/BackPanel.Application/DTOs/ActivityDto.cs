namespace BackPanel.Application.DTOs;

public class ActivityDto : DtoBase
{
    public int UserId { get; set; }
    public string? EffectedTable { get; set; }
    public int EffectedRowId { get; set; }
    public string? Action { get; set; }
}