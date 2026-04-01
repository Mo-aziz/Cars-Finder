namespace WebApplication3.DTOs;

public class CarProfileListDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string? Color { get; set; }
    public decimal Price { get; set; }
    public string CarBrand { get; set; } = string.Empty;
    public string CarModel { get; set; } = string.Empty;
}
