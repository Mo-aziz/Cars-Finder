using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.DTOs;
using WebApplication3.Interfaces;

namespace WebApplication3.Services;

public class CarEngineService : ICarEngineService
{
    private readonly ApplicationDbContext _context;

    public CarEngineService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CarEngineListDto>> GetAllAsync()
    {
        return await _context.CarEngines
            .Include(ce => ce.Car)
            .Include(ce => ce.Engine)
            .Select(ce => new CarEngineListDto
            {
                CarId = ce.CarId,
                EngineId = ce.EngineId,
                InstallationDate = ce.InstallationDate,
                CarBrand = ce.Car != null ? ce.Car.Brand : "Unknown",
                CarModel = ce.Car != null ? ce.Car.Model : "Unknown",
                EngineType = ce.Engine != null ? ce.Engine.Type : "Unknown"
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<CarEngineDetailsDto?> GetByIdAsync(int carId, int engineId)
    {
        return await _context.CarEngines
            .Include(ce => ce.Car)
            .Include(ce => ce.Engine)
            .Where(ce => ce.CarId == carId && ce.EngineId == engineId)
            .Select(ce => new CarEngineDetailsDto
            {
                CarId = ce.CarId,
                EngineId = ce.EngineId,
                InstallationDate = ce.InstallationDate,
                Notes = ce.Notes,
                CarBrand = ce.Car != null ? ce.Car.Brand : "Unknown",
                CarModel = ce.Car != null ? ce.Car.Model : "Unknown",
                CarYear = ce.Car != null ? ce.Car.Year : 0,
                EngineType = ce.Engine != null ? ce.Engine.Type : "Unknown",
                EngineHorsePower = ce.Engine != null ? ce.Engine.HorsePower : 0
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<CarEngineDetailsDto?> GetCarEngineDetailsAsync(int carId, int engineId)
    {
        return await GetByIdAsync(carId, engineId);
    }

    public async Task<CarEngineDetailsDto> CreateAsync(CarEngineCreateDto carEngineDto)
    {
        var carEngine = new CarEngine
        {
            CarId = carEngineDto.CarId,
            EngineId = carEngineDto.EngineId,
            InstallationDate = carEngineDto.InstallationDate,
            Notes = carEngineDto.Notes
        };

        _context.CarEngines.Add(carEngine);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(carEngine.CarId, carEngine.EngineId) ?? new CarEngineDetailsDto { CarId = carEngine.CarId, EngineId = carEngine.EngineId };
    }

    public async Task<CarEngineDetailsDto?> UpdateAsync(int carId, int engineId, CarEngineUpdateDto carEngineDto)
    {
        var carEngine = await _context.CarEngines
            .FirstOrDefaultAsync(ce => ce.CarId == carId && ce.EngineId == engineId);
        if (carEngine != null)
        {
            carEngine.InstallationDate = carEngineDto.InstallationDate;
            carEngine.Notes = carEngineDto.Notes;
            
            _context.CarEngines.Update(carEngine);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(carEngine.CarId, carEngine.EngineId);
        }
        return null;
    }

    public async Task<bool> DeleteAsync(int carId, int engineId)
    {
        var carEngine = await _context.CarEngines
            .FirstOrDefaultAsync(ce => ce.CarId == carId && ce.EngineId == engineId);
        if (carEngine != null)
        {
            _context.CarEngines.Remove(carEngine);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
