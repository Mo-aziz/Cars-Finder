using Microsoft.AspNetCore.Mvc;
using WebApplication3.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Controllers;

public class CarController : Controller, ICar
{
    private static List<Car> cars = new List<Car>();

    public List<Car> GetAll()
    {
        return cars;
    }

    public Car? GetById(int id)
    {
        return cars.FirstOrDefault(c => c.Id == id);
    }

    public void Add(Car car)
    {
        cars.Add(car);
    }

    public IActionResult Index()
    {
        var allCars = GetAll();
        return View(allCars);
    }

    public IActionResult Details(int id)
    {
        var car = GetById(id);
        if (car == null)
        {
            return NotFound();
        }
        return View(car);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Car car)
    {
        if (ModelState.IsValid)
        {
            Add(car);
            return RedirectToAction("Index");
        }
        return View(car);
    }
}
