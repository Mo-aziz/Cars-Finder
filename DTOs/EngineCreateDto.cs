using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.DTOs;

public class EngineCreateDto
{
    [Required]
    [StringLength(100)]
    public string Type { get; set; } = string.Empty;
    
    [Required]
    public int HorsePower { get; set; }
    
    [StringLength(50)]
    public string? FuelType { get; set; }
    
    [Column(TypeName = "decimal(6,2)")]
    public decimal? Displacement { get; set; }
}
