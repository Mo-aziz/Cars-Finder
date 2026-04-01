using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Controllers;

public class ManufacturerController : Controller
{
    private readonly IManufacturerService _manufacturerService;

    public ManufacturerController(IManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }

    // ==================== MVC Views ====================

    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<IActionResult> Index()
    {
        var manufacturers = await _manufacturerService.GetAllAsync();
        return View(manufacturers);
    }

    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<IActionResult> Details(int id)
    {
        var manufacturer = await _manufacturerService.GetManufacturerDetailsAsync(id);
        if (manufacturer == null)
        {
            return NotFound();
        }
        return View(manufacturer);
    }

    [Authorize(Roles = "Instructor,Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> Create(ManufacturerCreateDto manufacturerDto)
    {
        if (ModelState.IsValid)
        {
            await _manufacturerService.CreateAsync(manufacturerDto);
            return RedirectToAction(nameof(Index));
        }
        return View(manufacturerDto);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var manufacturer = await _manufacturerService.GetByIdAsync(id);
        if (manufacturer == null)
        {
            return NotFound();
        }
        
        var updateDto = new ManufacturerUpdateDto
        {
            Name = manufacturer.Name,
            Country = manufacturer.Country
        };
        return View(updateDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, ManufacturerUpdateDto manufacturerDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _manufacturerService.UpdateAsync(id, manufacturerDto);
            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        return View(manufacturerDto);
    }

   
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var manufacturer = await _manufacturerService.GetByIdAsync(id);
        if (manufacturer == null)
        {
            return NotFound();
        }
        return View(manufacturer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _manufacturerService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // ==================== API Endpoints ====================

    [HttpGet("api/[controller]")]
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<ManufacturerListDto>>> GetAllApi()
    {
        var manufacturers = await _manufacturerService.GetAllAsync();
        return Ok(manufacturers);
    }

    [HttpGet("api/[controller]/{id}")]
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<ActionResult<ManufacturerDetailsDto>> GetByIdApi(int id)
    {
        var manufacturer = await _manufacturerService.GetByIdAsync(id);
        if (manufacturer == null)
        {
            return NotFound();
        }
        return Ok(manufacturer);
    }

    [HttpPost("api/[controller]")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<ManufacturerDetailsDto>> CreateApi([FromBody] ManufacturerCreateDto manufacturerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var manufacturer = await _manufacturerService.CreateAsync(manufacturerDto);
        return CreatedAtAction(nameof(GetByIdApi), new { id = manufacturer.Id }, manufacturer);
    }

    [HttpPut("api/[controller]/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ManufacturerDetailsDto>> UpdateApi(int id, [FromBody] ManufacturerUpdateDto manufacturerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var manufacturer = await _manufacturerService.UpdateAsync(id, manufacturerDto);
        if (manufacturer == null)
        {
            return NotFound();
        }
        return Ok(manufacturer);
    }

    [HttpDelete("api/[controller]/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteApi(int id)
    {
        var result = await _manufacturerService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
