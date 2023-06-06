namespace BackPanel.Application.DTOs;
public class CompanyInfoDto : DtoBase
{
    public string? CompanyName { get; set; }

    public string? Address { get; set; }

    public int LogoId { get; set; }

    public ImageDto? Logo { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Fax { get; set; }

    public string? AboutUs { get; set; }
}