using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Utilities;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public AuthController(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        // Check if user exists in database
        var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
        
        if (user == null || !PasswordHasher.Verify(model.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        // Generate JWT token with user's role from database
        var token = GenerateJwtToken(user.Username, user.Role);

        // Store JWT token in HTTP-only cookie
        var isProduction = !HttpContext.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() ?? false;
        Response.Cookies.Append("token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = isProduction,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });

        return Ok(new { message = "Login successful", username = user.Username, role = user.Role });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Remove the JWT token from cookie
        Response.Cookies.Delete("token");
        return Ok(new { message = "Logout successful" });
    }

    [HttpPost("register")]
    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest(new { message = "Username and password are required" });
        }

        // Check if user already exists
        if (_context.Users.Any(u => u.Username == model.Username))
        {
            return BadRequest(new { message = "Username already exists" });
        }

        // Validate role
        var validRoles = new[] { "Admin", "Instructor", "User" };
        if (!validRoles.Contains(model.Role))
        {
            return BadRequest(new { message = "Invalid role. Must be Admin, Instructor, or User" });
        }

        // Create new user with hashed password
        var newUser = new User
        {
            Username = model.Username,
            PasswordHash = PasswordHasher.Hash(model.Password),
            Role = model.Role,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully", username = newUser.Username, role = newUser.Role });
    }

    [HttpPost("refresh-token")]
    [Authorize]
    public IActionResult RefreshToken()
    {
        var username = User.Identity?.Name;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(role))
        {
            return Unauthorized(new { message = "Invalid token claims" });
        }

        // Generate new JWT token
        var newToken = GenerateJwtToken(username, role);

        // Update the HTTP-only cookie with new token
        var isProduction = !HttpContext.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() ?? false;
        Response.Cookies.Append("token", newToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = isProduction,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });

        return Ok(new { message = "Token refreshed successfully" });
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
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "User"; // Default role
}
