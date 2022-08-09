namespace BackPanel.Application.DTOs;

public class MessageDto : DtoBase
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Content { get; set; }
    public string? Phone { get; set; }
}