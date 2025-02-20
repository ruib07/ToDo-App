using ToDo.Server.Models;

namespace ToDo.Server.Repositories.Interfaces;

public interface IUsersRepository
{
    Task<Users> GetUserById(Guid userId);
    Task<Users> GetUserByEmail(string email);
    Task<Users> CreateUser(Users user);
    Task UpdateUser(Users user);
    Task DeleteUser(Guid userId);
}
