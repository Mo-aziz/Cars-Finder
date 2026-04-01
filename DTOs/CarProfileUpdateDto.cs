using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs;

public class CarProfileUpdateDto
{
    [Required]
    public int CarId { get; set; }
    
    [StringLength(50)]
    public string? Color { get; set; }
    
    [Range(0, 999999.99)]
    public decimal Price { get; set; }
    
    [StringLength(1000)]
    public string? Description { get; set; }
}
