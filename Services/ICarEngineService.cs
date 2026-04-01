using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Interfaces;

public interface ICarEngineService
{
    Task<List<CarEngineListDto>> GetAllAsync();
    Task<CarEngineDetailsDto?> GetByIdAsync(int carId, int engineId);
    Task<CarEngineDetailsDto?> GetCarEngineDetailsAsync(int carId, int engineId);
    Task<CarEngineDetailsDto> CreateAsync(CarEngineCreateDto carEngineDto);
    Task<CarEngineDetailsDto?> UpdateAsync(int carId, int engineId, CarEngineUpdateDto carEngineDto);
    Task<bool> DeleteAsync(int carId, int engineId);
}
