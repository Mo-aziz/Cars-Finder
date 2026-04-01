namespace WebApplication3.DTOs;

public class CarEngineDetailsDto
{
    public int CarId { get; set; }
    public int EngineId { get; set; }
    public DateTime InstallationDate { get; set; }
    public string? Notes { get; set; }
    public string CarBrand { get; set; } = string.Empty;
    public string CarModel { get; set; } = string.Empty;
    public int CarYear { get; set; }
    public string EngineType { get; set; } = string.Empty;
    public int EngineHorsePower { get; set; }
    public string? EngineFuelType { get; set; }
    public decimal? EngineDisplacement { get; set; }
}
