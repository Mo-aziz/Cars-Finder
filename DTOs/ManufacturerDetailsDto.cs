namespace WebApplication3.DTOs;

public class ManufacturerDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? Description { get; set; }
    public int CarCount { get; set; }
}
