using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication3.Interfaces;
using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private readonly ICarService _carService;
    private readonly IManufacturerService _manufacturerService;

    public CarController(ICarService carService, IManufacturerService manufacturerService)
    {
        _carService = carService;
        _manufacturerService = manufacturerService;
    }

    // ==================== API Endpoints ====================

    // GET: api/car
    [HttpGet]
    [Authorize(Roles = "User,Employee,Admin")]
    public async Task<ActionResult<IEnumerable<CarListDto>>> GetAll()
    {
        var cars = await _carService.GetAllAsync();
        return Ok(cars);
    }

    // GET: api/car/{id}
    [HttpGet("{id}")]
    [Authorize(Roles = "User,Employee,Admin")]
    public async Task<ActionResult<CarDetailsDto>> GetById(int id)
    {
        var car = await _carService.GetByIdAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        return Ok(car);
    }

    // POST: api/car
    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<CarDetailsDto>> Create([FromBody] CarCreateDto carDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var car = await _carService.CreateAsync(carDto);
        return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
    }

    // PUT: api/car/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CarDetailsDto>> Update(int id, [FromBody] CarUpdateDto carDto)
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

    // DELETE: api/car/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _carService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
