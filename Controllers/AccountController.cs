using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Utilities;

namespace WebApplication3.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AccountController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // GET: Account/Login
    public IActionResult Login()
    {
        // If user is already logged in, redirect to home
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Username and password are required";
            return View();
        }

        // Check if user exists in database
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null || !PasswordHasher.Verify(password, user.PasswordHash))
        {
            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // Generate JWT token
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

        return RedirectToAction("Index", "Home");
    }

    // GET: Account/SignUp
    public IActionResult SignUp()
    {
        // If user is already logged in, redirect to home
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    // POST: Account/SignUp
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(string username, string password, string confirmPassword, string role)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(username))
        {
            ViewBag.Error = "Username is required";
            return View();
        }

        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
        {
            ViewBag.Error = "Password and confirm password are required";
            return View();
        }

        if (password != confirmPassword)
        {
            ViewBag.Error = "Passwords do not match";
            return View();
        }

        if (password.Length < 6)
        {
            ViewBag.Error = "Password must be at least 6 characters long";
            return View();
        }

        // Check if username already exists
        if (_context.Users.Any(u => u.Username == username))
        {
            ViewBag.Error = "Username already exists. Please choose a different username";
            return View();
        }

        // Validate role
        if (string.IsNullOrWhiteSpace(role))
        {
            role = "User";
        }

        var validRoles = new[] { "Admin", "Instructor", "User" };
        if (!validRoles.Contains(role))
        {
            ViewBag.Error = "Invalid role selected";
            return View();
        }

        try
        {
            // Create new user with hashed password
            var newUser = new User
            {
                Username = username,
                PasswordHash = PasswordHasher.Hash(password),
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Auto-login the user after successful registration
            var token = GenerateJwtToken(newUser.Username, newUser.Role);

            var isProduction = !HttpContext.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() ?? false;
            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = isProduction,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            // Set cookie to authenticate the user
            var claimsIdentity = new System.Security.Principal.GenericIdentity(newUser.Username, "Forms");
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, newUser.Username),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, newUser.Username),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, newUser.Role)
            };
            claimsIdentity.AddClaims(claims);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"An error occurred during registration: {ex.Message}";
            return View();
        }
    }

    // GET: Account/Logout
    public IActionResult Logout()
    {
        // Remove the JWT token from cookie
        Response.Cookies.Delete("token");
        
        // Redirect to login page
        return RedirectToAction("Login");
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
