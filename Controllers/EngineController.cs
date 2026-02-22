using Microsoft.AspNetCore.Mvc;
using WebApplication3.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Controllers;

public class EngineController : Controller, IEngine
{
    private static List<Engine> engines = new List<Engine>();

    public List<Engine> GetAll()
    {
        return engines;
    }

    public Engine? GetById(int id)
    {
        return engines.FirstOrDefault(e => e.Id == id);
    }

    public void Add(Engine engine)
    {
        engines.Add(engine);
    }

    public IActionResult Index()
    {
        var allEngines = GetAll();
        return View(allEngines);
    }

    public IActionResult Details(int id)
    {
        var engine = GetById(id);
        if (engine == null)
        {
            return NotFound();
        }
        return View(engine);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Engine engine)
    {
        if (ModelState.IsValid)
        {
            Add(engine);
            return RedirectToAction("Index");
        }
        return View(engine);
    }
}
