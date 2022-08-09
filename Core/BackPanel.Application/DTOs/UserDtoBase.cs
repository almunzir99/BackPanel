using System.ComponentModel.DataAnnotations;

namespace BackPanel.Application.DTOs;

public class UserDtoBase : DtoBase
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public string? Photo { get; set; }
    public IList<NotificationDto> Notifications { get; set; } = new List<NotificationDto>(); 
}