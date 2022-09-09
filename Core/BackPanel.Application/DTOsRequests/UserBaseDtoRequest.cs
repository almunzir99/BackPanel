using System.ComponentModel.DataAnnotations;

namespace BackPanel.Application.DTOsRequests;

public class UserBaseDtoRequest
{
    [Required] [MaxLength(30)] public string? Username { get; set; }
    [Required] public string? Email { get; set; }

    [Required]
    [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}")]
    public string? Phone { get; set; }
    public string? Password { get; set; }
    public string? Image { get; set; }
    public int? RoleId { get; set; }
}