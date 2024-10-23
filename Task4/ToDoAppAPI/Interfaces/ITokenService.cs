using ToDoAppAPI.Models;

namespace ToDoAppAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Employee employee);
    }
}
