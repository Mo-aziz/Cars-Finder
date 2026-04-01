namespace WebApplication3.DTOs;

public class CarEngineListDto
{
    public int CarId { get; set; }
    public int EngineId { get; set; }
    public DateTime InstallationDate { get; set; }
    public string CarBrand { get; set; } = string.Empty;
    public string CarModel { get; set; } = string.Empty;
    public string EngineType { get; set; } = string.Empty;
}
