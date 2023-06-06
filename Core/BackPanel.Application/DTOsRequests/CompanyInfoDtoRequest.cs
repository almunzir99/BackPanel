using System.ComponentModel.DataAnnotations;

namespace BackPanel.Application.DTOsRequests;
public class CompanyInfoDtoRequest
{
    [Required]
    public string? CompanyName { get; set; }

    [Required]
    public string? Address { get; set; }

    public int LogoId { get; set; }

    public ImageDtoRequest? Logo { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Fax { get; set; }

    public string? AboutUs { get; set; }
}