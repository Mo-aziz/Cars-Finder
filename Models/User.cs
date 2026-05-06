using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Role { get; set; } = "User"; // Default role is "User"

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
