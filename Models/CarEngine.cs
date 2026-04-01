using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models;

public class CarEngine
{
    // Composite key (CarId + EngineId) as configured in DbContext
    public int CarId { get; set; }
    public int EngineId { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime InstallationDate { get; set; } = DateTime.Today;
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    // Navigation properties for many-to-many relationship
    public virtual Car Car { get; set; } = null!;
    public virtual Engine Engine { get; set; } = null!;
}
