namespace BackPanel.Application.DTOs;

public class AdminDto : UserDtoBase
{
    public bool IsManager { get; set; }
    public string? Image { get; set; }
    public int? RoleId { get; set; }
    public RoleDto? Role { get; set; }
}