using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Interfaces;

public interface IManufacturerService
{
    Task<List<ManufacturerListDto>> GetAllAsync();
    Task<ManufacturerDetailsDto?> GetByIdAsync(int id);
    Task<ManufacturerDetailsDto?> GetManufacturerDetailsAsync(int id);
    Task<ManufacturerDetailsDto> CreateAsync(ManufacturerCreateDto manufacturerDto);
    Task<ManufacturerDetailsDto?> UpdateAsync(int id, ManufacturerUpdateDto manufacturerDto);
    Task<bool> DeleteAsync(int id);
}
