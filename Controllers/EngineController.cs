using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Controllers;

public class EngineController : Controller
{
    private readonly IEngineService _engineService;

    public EngineController(IEngineService engineService)
    {
        _engineService = engineService;
    }

    // ==================== MVC Views ====================

    public async Task<IActionResult> Index()
    {
        var engines = await _engineService.GetAllAsync();
        return View(engines);
    }

    public async Task<IActionResult> Details(int id)
    {
        var engine = await _engineService.GetEngineDetailsAsync(id);
        if (engine == null)
        {
            return NotFound();
        }
        return View(engine);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EngineCreateDto engineDto)
    {
        if (ModelState.IsValid)
        {
            await _engineService.CreateAsync(engineDto);
            return RedirectToAction(nameof(Index));
        }
        return View(engineDto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var engine = await _engineService.GetByIdAsync(id);
        if (engine == null)
        {
            return NotFound();
        }
        
        var updateDto = new EngineUpdateDto
        {
            Type = engine.Type,
            HorsePower = engine.HorsePower
        };
        return View(updateDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EngineUpdateDto engineDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _engineService.UpdateAsync(id, engineDto);
            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        return View(engineDto);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var engine = await _engineService.GetByIdAsync(id);
        if (engine == null)
        {
            return NotFound();
        }
        return View(engine);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _engineService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // ==================== API Endpoints ====================

    [HttpGet("api/[controller]")]
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<EngineListDto>>> GetAllApi()
    {
        var engines = await _engineService.GetAllAsync();
        return Ok(engines);
    }

    [HttpGet("api/[controller]/{id}")]
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<ActionResult<EngineDetailsDto>> GetByIdApi(int id)
    {
        var engine = await _engineService.GetByIdAsync(id);
        if (engine == null)
        {
            return NotFound();
        }
        return Ok(engine);
    }

    [HttpPost("api/[controller]")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<EngineDetailsDto>> CreateApi([FromBody] EngineCreateDto engineDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var engine = await _engineService.CreateAsync(engineDto);
        return CreatedAtAction(nameof(GetByIdApi), new { id = engine.Id }, engine);
    }

    [HttpPut("api/[controller]/{id}")]
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

    [HttpDelete("api/[controller]/{id}")]
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
