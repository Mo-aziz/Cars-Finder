using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs;

public class CarCreateDto
{
    [Required]
    [StringLength(100)]
    public string Brand { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Model { get; set; } = string.Empty;
    
    [Required]
    [Range(1900, 2100)]
    public int Year { get; set; }
    
    [Required]
    public int ManufacturerId { get; set; }
}
