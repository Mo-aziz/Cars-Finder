using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.DTOs;
using WebApplication3.Interfaces;

namespace WebApplication3.Services;

public class EngineService : IEngineService
{
    private readonly ApplicationDbContext _context;

    public EngineService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EngineListDto>> GetAllAsync()
    {
        return await _context.Engines
            .Select(e => new EngineListDto
            {
                Id = e.Id,
                Type = e.Type,
                HorsePower = e.HorsePower
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<EngineDetailsDto?> GetByIdAsync(int id)
    {
        return await _context.Engines
            .Include(e => e.CarEngines)
            .Where(e => e.Id == id)
            .Select(e => new EngineDetailsDto
            {
                Id = e.Id,
                Type = e.Type,
                HorsePower = e.HorsePower,
                CarCount = e.CarEngines != null ? e.CarEngines.Count : 0
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<EngineDetailsDto?> GetEngineDetailsAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<EngineDetailsDto> CreateAsync(EngineCreateDto engineDto)
    {
        var engine = new Engine
        {
            Type = engineDto.Type,
            HorsePower = engineDto.HorsePower
        };

        _context.Engines.Add(engine);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(engine.Id) ?? new EngineDetailsDto { Id = engine.Id, Type = engine.Type, HorsePower = engine.HorsePower };
    }

    public async Task<EngineDetailsDto?> UpdateAsync(int id, EngineUpdateDto engineDto)
    {
        var engine = await _context.Engines.FindAsync(id);
        if (engine != null)
        {
            engine.Type = engineDto.Type;
            engine.HorsePower = engineDto.HorsePower;
            
            _context.Engines.Update(engine);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(engine.Id);
        }
        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var engine = await _context.Engines.FindAsync(id);
        if (engine != null)
        {
            _context.Engines.Remove(engine);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
