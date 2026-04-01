using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;

public class Engine
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Type { get; set; } = string.Empty;
    
    [Range(1, 2000)]
    public int HorsePower { get; set; }
    
    // Navigation properties
    public virtual ICollection<CarEngine> CarEngines { get; set; } = new List<CarEngine>();
}
