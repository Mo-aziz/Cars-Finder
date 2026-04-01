using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.DTOs;
using WebApplication3.Interfaces;

namespace WebApplication3.Services;

public class CarService : ICarService
{
    private readonly ApplicationDbContext _context;

    public CarService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarListDto>> GetAllAsync()
    {
        return await _context.Cars
            .Include(c => c.Manufacturer)
            .Select(c => new CarListDto
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                ManufacturerName = c.Manufacturer != null ? c.Manufacturer.Name : "Unknown"
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<CarDetailsDto?> GetByIdAsync(int id)
    {
        return await _context.Cars
            .Include(c => c.Manufacturer)
            .Include(c => c.CarProfile)
            .Where(c => c.Id == id)
            .Select(c => new CarDetailsDto
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                ManufacturerId = c.ManufacturerId,
                ManufacturerName = c.Manufacturer != null ? c.Manufacturer.Name : "Unknown",
                Color = c.CarProfile != null ? c.CarProfile.Color ?? "N/A" : "N/A",
                Price = c.CarProfile != null ? c.CarProfile.Price : 0,
                Description = c.CarProfile != null ? c.CarProfile.Description ?? "N/A" : "N/A"
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<CarDetailsDto?> GetCarDetailsAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<CarDetailsDto> CreateAsync(CarCreateDto carDto)
    {
        var car = new Car
        {
            Brand = carDto.Brand,
            Model = carDto.Model,
            Year = carDto.Year,
            ManufacturerId = carDto.ManufacturerId
        };

        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(car.Id) ?? new CarDetailsDto { Id = car.Id, Brand = car.Brand, Model = car.Model, Year = car.Year };
    }

    public async Task<CarDetailsDto?> UpdateAsync(int id, CarUpdateDto carDto)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car != null)
        {
            car.Brand = carDto.Brand;
            car.Model = carDto.Model;
            car.Year = carDto.Year;
            car.ManufacturerId = carDto.ManufacturerId;
            
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(car.Id);
        }
        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car != null)
        {
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
