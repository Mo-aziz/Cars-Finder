using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.DTOs;
using System.IO;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarProfilesController : Controller
{
    private readonly ICarProfileService _carProfileService;
    private readonly ICarService _carService;

    public CarProfilesController(ICarProfileService carProfileService, ICarService carService)
    {
        _carProfileService = carProfileService;
        _carService = carService;
    }

    // ==================== API Endpoints ====================

    [HttpGet]
    [Authorize(Roles = "User,Employee,Admin")]
    public async Task<ActionResult<IEnumerable<CarProfileListDto>>> GetAllApi()
    {
        var carProfiles = await _carProfileService.GetAllAsync();
        return Ok(carProfiles);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "User,Employee,Admin")]
    public async Task<ActionResult<CarProfileDetailsDto>> GetByIdApi(int id)
    {
        var carProfile = await _carProfileService.GetByIdAsync(id);
        if (carProfile == null)
        {
            return NotFound();
        }
        return Ok(carProfile);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<CarProfileDetailsDto>> CreateApi([FromBody] CarProfileCreateDto carProfileDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var carProfile = await _carProfileService.CreateAsync(carProfileDto);
        return CreatedAtAction(nameof(GetByIdApi), new { id = carProfile.Id }, carProfile);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<CarProfileDetailsDto>> UpdateApi(int id, [FromBody] CarProfileUpdateDto carProfileDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var carProfile = await _carProfileService.UpdateAsync(id, carProfileDto);
        if (carProfile == null)
        {
            return NotFound();
        }
        return Ok(carProfile);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteApi(int id)
    {
        var result = await _carProfileService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPost("upload-photo")]
    [Authorize(Roles = "Admin,Employee")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<ActionResult> UploadPhoto([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded." });
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(new { message = "Invalid file type. Allowed: jpg, jpeg, png, webp, gif." });
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "carprofiles");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var photoUrl = $"/uploads/carprofiles/{fileName}";
        return Ok(new { photoUrl });
    }
}
