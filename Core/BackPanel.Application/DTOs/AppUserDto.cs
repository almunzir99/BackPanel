namespace BackPanel.Application.DTOs;

public class AppUserDto : DtoBase
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Image { get; set; }
    public bool IsManager { get; set; }
    public string? Token { get; set; }
    public int? RoleId { get; set; }
    public RoleDto? Role { get; set; }
}
