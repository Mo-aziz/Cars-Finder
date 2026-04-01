using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Controllers;

public class CarController : Controller
{
    private readonly ICarService _carService;
    private readonly IManufacturerService _manufacturerService;

    public CarController(ICarService carService, IManufacturerService manufacturerService)
    {
        _carService = carService;
        _manufacturerService = manufacturerService;
    }

    // ==================== MVC Views ====================

    // GET: /Car (Index View)
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<IActionResult> Index()
    {
        var cars = await _carService.GetAllAsync();
        return View(cars);
    }

    // GET: /Car/Details/5
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<IActionResult> Details(int id)
    {
        var car = await _carService.GetCarDetailsAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        return View(car);
    }

    // GET: /Car/Create
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> Create()
    {
        var manufacturers = await _manufacturerService.GetAllAsync();
        ViewBag.Manufacturers = manufacturers.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
        return View();
    }

    // POST: /Car/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> Create(CarCreateDto carDto)
    {
        if (ModelState.IsValid)
        {
            await _carService.CreateAsync(carDto);
            return RedirectToAction(nameof(Index));
        }
        var manufacturers = await _manufacturerService.GetAllAsync();
        ViewBag.Manufacturers = manufacturers.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
        return View(carDto);
    }

    // GET: /Car/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var car = await _carService.GetByIdAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        var manufacturers = await _manufacturerService.GetAllAsync();
        ViewBag.Manufacturers = manufacturers.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
        
        var updateDto = new CarUpdateDto
        {
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            ManufacturerId = car.ManufacturerId
        };
        return View(updateDto);
    }

    // POST: /Car/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, CarUpdateDto carDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _carService.UpdateAsync(id, carDto);
            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        var manufacturers = await _manufacturerService.GetAllAsync();
        ViewBag.Manufacturers = manufacturers.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
        return View(carDto);
    }

    // GET: /Car/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var car = await _carService.GetByIdAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        return View(car);
    }

    // POST: /Car/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _carService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // ==================== API Endpoints ====================

    // GET: api/Car
    [HttpGet("api/[controller]")]
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<CarListDto>>> GetAllApi()
    {
        var cars = await _carService.GetAllAsync();
        return Ok(cars);
    }

    // GET: api/Car/5
    [HttpGet("api/[controller]/{id}")]
    [Authorize(Roles = "User,Instructor,Admin")]
    public async Task<ActionResult<CarDetailsDto>> GetByIdApi(int id)
    {
        var car = await _carService.GetByIdAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        return Ok(car);
    }

    // POST: api/Car
    [HttpPost("api/[controller]")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<ActionResult<CarDetailsDto>> CreateApi([FromBody] CarCreateDto carDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var car = await _carService.CreateAsync(carDto);
        return CreatedAtAction(nameof(GetByIdApi), new { id = car.Id }, car);
    }

    // PUT: api/Car/5
    [HttpPut("api/[controller]/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CarDetailsDto>> UpdateApi(int id, [FromBody] CarUpdateDto carDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var car = await _carService.UpdateAsync(id, carDto);
        if (car == null)
        {
            return NotFound();
        }
        return Ok(car);
    }

    // DELETE: api/Car/5
    [HttpDelete("api/[controller]/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteApi(int id)
    {
        var result = await _carService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
