using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarEnginesController : Controller
{
    private readonly ICarEngineService _carEngineService;
    private readonly ICarService _carService;
    private readonly IEngineService _engineService;

    public CarEnginesController(ICarEngineService carEngineService, ICarService carService, IEngineService engineService)
    {
        _carEngineService = carEngineService;
        _carService = carService;
        _engineService = engineService;
    }

    // ==================== API Endpoints ====================

    [HttpGet]
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<CarEngineListDto>>> GetAllApi()
    {
        var carEngines = await _carEngineService.GetAllAsync();
        return Ok(carEngines);
    }

    [HttpGet("{carId}/{engineId}")]
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<ActionResult<CarEngineDetailsDto>> GetByIdApi(int carId, int engineId)
    {
        var carEngine = await _carEngineService.GetByIdAsync(carId, engineId);
        if (carEngine == null)
        {
            return NotFound();
        }
        return Ok(carEngine);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<CarEngineDetailsDto>> CreateApi([FromBody] CarEngineCreateDto carEngineDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var carEngine = await _carEngineService.CreateAsync(carEngineDto);
        return CreatedAtAction(nameof(GetByIdApi), new { carId = carEngine.CarId, engineId = carEngine.EngineId }, carEngine);
    }

    [HttpPut("{carId}/{engineId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CarEngineDetailsDto>> UpdateApi(int carId, int engineId, [FromBody] CarEngineUpdateDto carEngineDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var carEngine = await _carEngineService.UpdateAsync(carId, engineId, carEngineDto);
        if (carEngine == null)
        {
            return NotFound();
        }
        return Ok(carEngine);
    }

    [HttpDelete("{carId}/{engineId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteApi(int carId, int engineId)
    {
        var result = await _carEngineService.DeleteAsync(carId, engineId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
