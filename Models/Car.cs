using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models;

public class Car
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Brand { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Model { get; set; } = string.Empty;
    
    [Range(1900, 2100)]
    public int Year { get; set; }
    
    public int ManufacturerId { get; set; }
    
    // Navigation properties
    [ForeignKey("ManufacturerId")]
    public virtual Manufacturer? Manufacturer { get; set; }
    
    public virtual CarProfile? CarProfile { get; set; }
    public virtual ICollection<CarEngine> CarEngines { get; set; } = new List<CarEngine>();
}
