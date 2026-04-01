using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models;

public class CarProfile
{
    public int Id { get; set; }
    public int CarId { get; set; }
    
    [StringLength(50)]
    public string? Color { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    // Navigation properties
    public virtual Car? Car { get; set; }
}
