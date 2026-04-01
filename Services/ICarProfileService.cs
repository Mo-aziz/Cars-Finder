using WebApplication3.Models;
using WebApplication3.DTOs;

namespace WebApplication3.Interfaces;

public interface ICarProfileService
{
    Task<List<CarProfileListDto>> GetAllAsync();
    Task<CarProfileDetailsDto?> GetByIdAsync(int id);
    Task<CarProfileDetailsDto?> GetCarProfileDetailsAsync(int id);
    Task<CarProfileDetailsDto> CreateAsync(CarProfileCreateDto carProfileDto);
    Task<CarProfileDetailsDto?> UpdateAsync(int id, CarProfileUpdateDto carProfileDto);
    Task<bool> DeleteAsync(int id);
}
