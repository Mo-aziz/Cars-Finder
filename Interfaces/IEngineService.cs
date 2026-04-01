using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Interfaces;

public interface IEngineService
{
    Task<List<EngineListDto>> GetAllAsync();
    Task<EngineDetailsDto?> GetByIdAsync(int id);
    Task<EngineDetailsDto?> GetEngineDetailsAsync(int id);
    Task<EngineDetailsDto> CreateAsync(EngineCreateDto engineDto);
    Task<EngineDetailsDto?> UpdateAsync(int id, EngineUpdateDto engineDto);
    Task<bool> DeleteAsync(int id);
}
