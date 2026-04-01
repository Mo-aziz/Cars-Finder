using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Interfaces;

public interface ICarService
{
    Task<IEnumerable<CarListDto>> GetAllAsync();
    Task<CarDetailsDto?> GetByIdAsync(int id);
    Task<CarDetailsDto?> GetCarDetailsAsync(int id);
    Task<CarDetailsDto> CreateAsync(CarCreateDto carDto);
    Task<CarDetailsDto?> UpdateAsync(int id, CarUpdateDto carDto);
    Task<bool> DeleteAsync(int id);
}
