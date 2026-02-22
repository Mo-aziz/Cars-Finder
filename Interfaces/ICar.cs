using WebApplication3.Models;

namespace WebApplication3.Interfaces;

public interface ICar
{
    List<Car> GetAll();
    Car? GetById(int id);
    void Add(Car car);
}
