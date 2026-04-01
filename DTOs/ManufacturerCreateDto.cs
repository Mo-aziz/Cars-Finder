using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs;

public class ManufacturerCreateDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? Country { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
}
