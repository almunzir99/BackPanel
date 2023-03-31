using System.ComponentModel.DataAnnotations;

namespace BackPanel.Domain.Entities;

public class UserEntityBase : EntityBase
{
    [Required]
    [MaxLength(30)]
    public string? Username { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}")]
    public string? Phone { get; set; }
    [Required]
    public byte[]? PasswordHash { get; set; }
    [Required]
    public byte[]? PasswordSalt { get; set; }
    public string? Image { get; set; }
    public IList<Notification> Notifications { get; set; } = new List<Notification>();
}