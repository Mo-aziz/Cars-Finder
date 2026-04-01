using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs;

public class CarEngineCreateDto
{
    [Required]
    public int CarId { get; set; }
    
    [Required]
    public int EngineId { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime InstallationDate { get; set; } = DateTime.Today;
    
    [StringLength(500)]
    public string? Notes { get; set; }
}
