namespace WebApplication3.DTOs;

public class CarListDto
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string ManufacturerName { get; set; } = string.Empty;
}
