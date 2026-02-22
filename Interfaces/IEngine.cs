using WebApplication3.Models;

namespace WebApplication3.Interfaces;

public interface IEngine
{
    List<Engine> GetAll();
    Engine? GetById(int id);
    void Add(Engine engine);
}
