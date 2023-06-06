using System.ComponentModel.DataAnnotations;

namespace BackPanel.Domain.Entities;

public class CompanyInfo : EntityBase
{
    [Required]
    public string? CompanyName { get; set; }
    [Required]
    public string? Address { get; set; }
    public int LogoId { get; set; }
    public Image? Logo { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Fax { get; set; }
    public string? AboutUs { get; set; }
}