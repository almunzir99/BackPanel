using System.ComponentModel.DataAnnotations;

namespace BackPanel.Application.DTOsRequests;
public class ImageDtoRequest
{
    [Required]
    public string? Path { get; set; }
}