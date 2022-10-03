using System.ComponentModel.DataAnnotations;

namespace BackPanel.Application.DTOs;

public class UserDtoBase : DtoBase
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Image { get; set; }
    public string? Token { get; set; }
}