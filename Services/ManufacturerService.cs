using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.DTOs;
using WebApplication3.Interfaces;

namespace WebApplication3.Services;

public class ManufacturerService : IManufacturerService
{
    private readonly ApplicationDbContext _context;

    public ManufacturerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ManufacturerListDto>> GetAllAsync()
    {
        return await _context.Manufacturers
            .Select(m => new ManufacturerListDto
            {
                Id = m.Id,
                Name = m.Name,
                Country = m.Country
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ManufacturerDetailsDto?> GetByIdAsync(int id)
    {
        return await _context.Manufacturers
            .Include(m => m.Cars)
            .Where(m => m.Id == id)
            .Select(m => new ManufacturerDetailsDto
            {
                Id = m.Id,
                Name = m.Name,
                Country = m.Country,
                Description = m.Description,
                CarCount = m.Cars != null ? m.Cars.Count : 0
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<ManufacturerDetailsDto?> GetManufacturerDetailsAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<ManufacturerDetailsDto> CreateAsync(ManufacturerCreateDto manufacturerDto)
    {
        var manufacturer = new Manufacturer
        {
            Name = manufacturerDto.Name,
            Country = manufacturerDto.Country,
            Description = manufacturerDto.Description
        };

        _context.Manufacturers.Add(manufacturer);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(manufacturer.Id) ?? new ManufacturerDetailsDto { Id = manufacturer.Id, Name = manufacturer.Name };
    }

    public async Task<ManufacturerDetailsDto?> UpdateAsync(int id, ManufacturerUpdateDto manufacturerDto)
    {
        var manufacturer = await _context.Manufacturers.FindAsync(id);
        if (manufacturer != null)
        {
            manufacturer.Name = manufacturerDto.Name;
            manufacturer.Country = manufacturerDto.Country;
            manufacturer.Description = manufacturerDto.Description;
            
            _context.Manufacturers.Update(manufacturer);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(manufacturer.Id);
        }
        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var manufacturer = await _context.Manufacturers.FindAsync(id);
        if (manufacturer != null)
        {
            _context.Manufacturers.Remove(manufacturer);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
