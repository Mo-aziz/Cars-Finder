using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        // For demo purposes, we'll use a simple validation
        // In a real application, you would validate against a database
        if (model.Username == "admin" && model.Password == "password")
        {
            var token = GenerateJwtToken(model.Username, "Admin");
            return Ok(new { token = token, expires_in = 3600, role = "Admin" });
        }
        else if (model.Username == "user" && model.Password == "password")
        {
            var token = GenerateJwtToken(model.Username, "User");
            return Ok(new { token = token, expires_in = 3600, role = "User" });
        }
        else if (model.Username == "instructor" && model.Password == "password")
        {
            var token = GenerateJwtToken(model.Username, "Instructor");
            return Ok(new { token = token, expires_in = 3600, role = "Instructor" });
        }

        return Unauthorized("Invalid username or password");
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterModel model)
    {
        // For demo purposes, we'll just return success
        // In a real application, you would create a user in the database
        return Ok(new { message = "User registered successfully", username = model.Username });
    }

    [HttpGet("profile")]
    [Authorize]
    public IActionResult GetProfile()
    {
        var username = User.Identity?.Name;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        
        return Ok(new { username = username, role = role });
    }

    private string GenerateJwtToken(string username, string role)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyForJWTTokenGeneration123456789";
        var key = Encoding.UTF8.GetBytes(secretKey);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Name, username)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpirationInMinutes"] ?? "60")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}

public class LoginModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterModel
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
