using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;

public class Manufacturer
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? Country { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    // Navigation properties
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
