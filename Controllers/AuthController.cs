using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        string? role = null;
        if (model.Username == "admin" && model.Password == "password")
        {
            role = "Admin";
        }
        else if (model.Username == "user" && model.Password == "password")
        {
            role = "User";
        }
        else if (model.Username == "instructor" && model.Password == "password")
        {
            role = "Instructor";
        }
        else
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        // Generate JWT token
        var token = GenerateJwtToken(model.Username, role);

        // Store JWT token in HTTP-only cookie
        var isProduction = !HttpContext.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() ?? false;
        Response.Cookies.Append("token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = isProduction,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });

        return Ok(new { message = "Login successful", username = model.Username, role = role });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Remove the JWT token from cookie
        Response.Cookies.Delete("token");
        return Ok(new { message = "Logout successful" });
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
        var jwtKey = _configuration["Jwt:Key"] ?? "MySecretKeyForJWTAuthenticationThat32CharactersLongMinimum1234567890";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "WebApplication3";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "WebApplication3Users";
        var key = Encoding.ASCII.GetBytes(jwtKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
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
