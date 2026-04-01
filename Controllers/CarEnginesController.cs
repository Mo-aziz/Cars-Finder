using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Controllers;

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

    // ==================== MVC Views ====================

    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<IActionResult> Index()
    {
        var carEngines = await _carEngineService.GetAllAsync();
        return View(carEngines);
    }

    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<IActionResult> Details(int carId, int engineId)
    {
        var carEngine = await _carEngineService.GetCarEngineDetailsAsync(carId, engineId);
        if (carEngine == null)
        {
            return NotFound();
        }
        return View(carEngine);
    }

    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> Create()
    {
        var cars = await _carService.GetAllAsync();
        var engines = await _engineService.GetAllAsync();
        ViewBag.Cars = cars.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Brand} {c.Model}" }).ToList();
        ViewBag.Engines = engines.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Type }).ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> Create(CarEngineCreateDto carEngineDto)
    {
        if (ModelState.IsValid)
        {
            await _carEngineService.CreateAsync(carEngineDto);
            return RedirectToAction(nameof(Index));
        }
        var cars = await _carService.GetAllAsync();
        var engines = await _engineService.GetAllAsync();
        ViewBag.Cars = cars.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Brand} {c.Model}" }).ToList();
        ViewBag.Engines = engines.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Type }).ToList();
        return View(carEngineDto);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int carId, int engineId)
    {
        var carEngine = await _carEngineService.GetByIdAsync(carId, engineId);
        if (carEngine == null)
        {
            return NotFound();
        }
        var cars = await _carService.GetAllAsync();
        var engines = await _engineService.GetAllAsync();
        ViewBag.Cars = cars.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Brand} {c.Model}" }).ToList();
        ViewBag.Engines = engines.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Type }).ToList();
        
        var updateDto = new CarEngineUpdateDto
        {
            InstallationDate = carEngine.InstallationDate
        };
        ViewBag.CarId = carId;
        ViewBag.EngineId = engineId;
        return View(updateDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int carId, int engineId, CarEngineUpdateDto carEngineDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _carEngineService.UpdateAsync(carId, engineId, carEngineDto);
            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        var cars = await _carService.GetAllAsync();
        var engines = await _engineService.GetAllAsync();
        ViewBag.Cars = cars.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Brand} {c.Model}" }).ToList();
        ViewBag.Engines = engines.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Type }).ToList();
        ViewBag.CarId = carId;
        ViewBag.EngineId = engineId;
        return View(carEngineDto);
    }

  
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int carId, int engineId)
    {
        var carEngine = await _carEngineService.GetByIdAsync(carId, engineId);
        if (carEngine == null)
        {
            return NotFound();
        }
        return View(carEngine);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int carId, int engineId)
    {
        await _carEngineService.DeleteAsync(carId, engineId);
        return RedirectToAction(nameof(Index));
    }

    // ==================== API Endpoints ====================

    [HttpGet("api/[controller]")]
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<CarEngineListDto>>> GetAllApi()
    {
        var carEngines = await _carEngineService.GetAllAsync();
        return Ok(carEngines);
    }

    [HttpGet("api/[controller]/{carId}/{engineId}")]
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

    [HttpPost("api/[controller]")]
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

    [HttpPut("api/[controller]/{carId}/{engineId}")]
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

    [HttpDelete("api/[controller]/{carId}/{engineId}")]
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
