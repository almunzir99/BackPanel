namespace BackPanel.Application.DTOsRequests;

public class AppUserDtoRequest
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public string? Image { get; set; }
    public bool IsManager { get; set; }
    public int? RoleId { get; set; }
}
