using Microsoft.AspNetCore.Mvc;
using WebApplication3.Interfaces;

namespace WebApplication3.Controllers;

public class HomeController : Controller
{
    private readonly ICarService _carService;
    private readonly IEngineService _engineService;
    private readonly IManufacturerService _manufacturerService;
    private readonly ICarProfileService _carProfileService;
    private readonly ICarEngineService _carEngineService;

    public HomeController(
        ICarService carService,
        IEngineService engineService,
        IManufacturerService manufacturerService,
        ICarProfileService carProfileService,
        ICarEngineService carEngineService)
    {
        _carService = carService;
        _engineService = engineService;
        _manufacturerService = manufacturerService;
        _carProfileService = carProfileService;
        _carEngineService = carEngineService;
    }

    public async Task<IActionResult> Index()
    {
        var cars = await _carService.GetAllAsync();
        var engines = await _engineService.GetAllAsync();
        var manufacturers = await _manufacturerService.GetAllAsync();
        var carProfiles = await _carProfileService.GetAllAsync();
        var carEngines = await _carEngineService.GetAllAsync();

        var dashboard = new DashboardViewModel
        {
            TotalCars = cars.Count(),
            TotalEngines = engines.Count,
            TotalManufacturers = manufacturers.Count,
            TotalCarProfiles = carProfiles.Count,
            TotalCarEngines = carEngines.Count,
            RecentCars = cars.Take(5).ToList()
        };

        return View(dashboard);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}

public class DashboardViewModel
{
    public int TotalCars { get; set; }
    public int TotalEngines { get; set; }
    public int TotalManufacturers { get; set; }
    public int TotalCarProfiles { get; set; }
    public int TotalCarEngines { get; set; }
    public IEnumerable<WebApplication3.DTOs.CarListDto> RecentCars { get; set; } = new List<WebApplication3.DTOs.CarListDto>();
}
