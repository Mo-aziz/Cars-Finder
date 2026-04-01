using WebApplication3.Models;

namespace WebApplication3.DTOs;

public class EngineDetailsDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public int HorsePower { get; set; }
    public int CarCount { get; set; }
}
