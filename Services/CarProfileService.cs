using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.DTOs;
using WebApplication3.Interfaces;

namespace WebApplication3.Services;

public class CarProfileService : ICarProfileService
{
    private readonly ApplicationDbContext _context;

    public CarProfileService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CarProfileListDto>> GetAllAsync()
    {
        return await _context.CarProfiles
            .Include(cp => cp.Car)
            .Select(cp => new CarProfileListDto
            {
                Id = cp.Id,
                CarId = cp.CarId,
                Color = cp.Color,
                Price = cp.Price,
                CarBrand = cp.Car != null ? cp.Car.Brand : "Unknown",
                CarModel = cp.Car != null ? cp.Car.Model : "Unknown"
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<CarProfileDetailsDto?> GetByIdAsync(int id)
    {
        return await _context.CarProfiles
            .Include(cp => cp.Car)
            .Where(cp => cp.Id == id)
            .Select(cp => new CarProfileDetailsDto
            {
                Id = cp.Id,
                CarId = cp.CarId,
                Color = cp.Color,
                Price = cp.Price,
                Description = cp.Description,
                CarBrand = cp.Car != null ? cp.Car.Brand : "Unknown",
                CarModel = cp.Car != null ? cp.Car.Model : "Unknown",
                CarYear = cp.Car != null ? cp.Car.Year : 0
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<CarProfileDetailsDto?> GetCarProfileDetailsAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<CarProfileDetailsDto> CreateAsync(CarProfileCreateDto carProfileDto)
    {
        var carProfile = new CarProfile
        {
            CarId = carProfileDto.CarId,
            Color = carProfileDto.Color,
            Price = carProfileDto.Price,
            Description = carProfileDto.Description
        };

        _context.CarProfiles.Add(carProfile);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(carProfile.Id) ?? new CarProfileDetailsDto { Id = carProfile.Id, CarId = carProfile.CarId };
    }

    public async Task<CarProfileDetailsDto?> UpdateAsync(int id, CarProfileUpdateDto carProfileDto)
    {
        var carProfile = await _context.CarProfiles.FindAsync(id);
        if (carProfile != null)
        {
            carProfile.CarId = carProfileDto.CarId;
            carProfile.Color = carProfileDto.Color;
            carProfile.Price = carProfileDto.Price;
            carProfile.Description = carProfileDto.Description;
            
            _context.CarProfiles.Update(carProfile);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(carProfile.Id);
        }
        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var carProfile = await _context.CarProfiles.FindAsync(id);
        if (carProfile != null)
        {
            _context.CarProfiles.Remove(carProfile);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
