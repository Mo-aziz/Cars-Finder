using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EngineController : Controller
{
    private readonly IEngineService _engineService;

    public EngineController(IEngineService engineService)
    {
        _engineService = engineService;
    }

    // ==================== API Endpoints ====================

    [HttpGet]
    [Authorize(Roles = "User,Employee,Admin")]
    public async Task<ActionResult<IEnumerable<EngineListDto>>> GetAllApi()
    {
        var engines = await _engineService.GetAllAsync();
        return Ok(engines);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "User,Employee,Admin")]
    public async Task<ActionResult<EngineDetailsDto>> GetByIdApi(int id)
    {
        var engine = await _engineService.GetByIdAsync(id);
        if (engine == null)
        {
            return NotFound();
        }
        return Ok(engine);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<EngineDetailsDto>> CreateApi([FromBody] EngineCreateDto engineDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var engine = await _engineService.CreateAsync(engineDto);
        return CreatedAtAction(nameof(GetByIdApi), new { id = engine.Id }, engine);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EngineDetailsDto>> UpdateApi(int id, [FromBody] EngineUpdateDto engineDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var engine = await _engineService.UpdateAsync(id, engineDto);
        if (engine == null)
        {
            return NotFound();
        }
        return Ok(engine);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteApi(int id)
    {
        var result = await _engineService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
