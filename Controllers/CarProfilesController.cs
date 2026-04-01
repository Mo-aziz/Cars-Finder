using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Controllers;

public class CarProfilesController : Controller
{
    private readonly ICarProfileService _carProfileService;
    private readonly ICarService _carService;

    public CarProfilesController(ICarProfileService carProfileService, ICarService carService)
    {
        _carProfileService = carProfileService;
        _carService = carService;
    }

    // ==================== MVC Views ====================

    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<IActionResult> Index()
    {
        var carProfiles = await _carProfileService.GetAllAsync();
        return View(carProfiles);
    }

    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<IActionResult> Details(int id)
    {
        var carProfile = await _carProfileService.GetCarProfileDetailsAsync(id);
        if (carProfile == null)
        {
            return NotFound();
        }
        return View(carProfile);
    }

    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> Create()
    {
        var cars = await _carService.GetAllAsync();
        ViewBag.Cars = cars.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Brand} {c.Model}" }).ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> Create(CarProfileCreateDto carProfileDto)
    {
        if (ModelState.IsValid)
        {
            await _carProfileService.CreateAsync(carProfileDto);
            return RedirectToAction(nameof(Index));
        }
        var cars = await _carService.GetAllAsync();
        ViewBag.Cars = cars.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Brand} {c.Model}" }).ToList();
        return View(carProfileDto);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var carProfile = await _carProfileService.GetByIdAsync(id);
        if (carProfile == null)
        {
            return NotFound();
        }
        var cars = await _carService.GetAllAsync();
        ViewBag.Cars = cars.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Brand} {c.Model}" }).ToList();
        
        var updateDto = new CarProfileUpdateDto
        {
            CarId = carProfile.CarId,
            Color = carProfile.Color,
            Price = carProfile.Price,
            Description = carProfile.Description
        };
        return View(updateDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, CarProfileUpdateDto carProfileDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _carProfileService.UpdateAsync(id, carProfileDto);
            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        var cars = await _carService.GetAllAsync();
        ViewBag.Cars = cars.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Brand} {c.Model}" }).ToList();
        return View(carProfileDto);
    }

   
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var carProfile = await _carProfileService.GetByIdAsync(id);
        if (carProfile == null)
        {
            return NotFound();
        }
        return View(carProfile);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _carProfileService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // ==================== API Endpoints ====================

    [HttpGet("api/[controller]")]
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<CarProfileListDto>>> GetAllApi()
    {
        var carProfiles = await _carProfileService.GetAllAsync();
        return Ok(carProfiles);
    }

    [HttpGet("api/[controller]/{id}")]
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<ActionResult<CarProfileDetailsDto>> GetByIdApi(int id)
    {
        var carProfile = await _carProfileService.GetByIdAsync(id);
        if (carProfile == null)
        {
            return NotFound();
        }
        return Ok(carProfile);
    }

    [HttpPost("api/[controller]")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<CarProfileDetailsDto>> CreateApi([FromBody] CarProfileCreateDto carProfileDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var carProfile = await _carProfileService.CreateAsync(carProfileDto);
        return CreatedAtAction(nameof(GetByIdApi), new { id = carProfile.Id }, carProfile);
    }

    [HttpPut("api/[controller]/{id}")]
    [Authorize(Roles = "Admin")]
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

    [HttpDelete("api/[controller]/{id}")]
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
}
