using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManufacturerController : Controller
{
    private readonly IManufacturerService _manufacturerService;

    public ManufacturerController(IManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }

    // ==================== API Endpoints ====================

    [HttpGet]
    [Authorize(Roles = "User,Employee,Admin")]
    public async Task<ActionResult<IEnumerable<ManufacturerListDto>>> GetAllApi()
    {
        var manufacturers = await _manufacturerService.GetAllAsync();
        return Ok(manufacturers);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "User,Employee,Admin")]
    public async Task<ActionResult<ManufacturerDetailsDto>> GetByIdApi(int id)
    {
        var manufacturer = await _manufacturerService.GetByIdAsync(id);
        if (manufacturer == null)
        {
            return NotFound();
        }
        return Ok(manufacturer);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<ManufacturerDetailsDto>> CreateApi([FromBody] ManufacturerCreateDto manufacturerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var manufacturer = await _manufacturerService.CreateAsync(manufacturerDto);
        return CreatedAtAction(nameof(GetByIdApi), new { id = manufacturer.Id }, manufacturer);
    }

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
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
