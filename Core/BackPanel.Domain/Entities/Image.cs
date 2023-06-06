using System.ComponentModel.DataAnnotations;

namespace BackPanel.Domain.Entities;

public class Image : EntityBase
{
    [Required]
    public string? Path { get; set; }
}