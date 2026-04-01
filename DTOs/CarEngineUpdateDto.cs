using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs;

public class CarEngineUpdateDto
{
    [Required]
    public int CarId { get; set; }
    
    [Required]
    public int EngineId { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime InstallationDate { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
}
