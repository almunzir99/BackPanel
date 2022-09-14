namespace BackPanel.Application.DTOs;

public class ActivityDto : DtoBase
{
     public int AdminId { get; set; }
    public AdminDto? Admin { get; set; }
    public string?  EffectedTable { get; set; }
    public int EffectedRowId { get; set; }
    public string? Action { get; set; }
}