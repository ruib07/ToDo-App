using ToDo.Server.Helpers;
using ToDo.Server.Models;
using ToDo.Server.Repositories.Interfaces;

namespace ToDo.Server.Services;

public class UsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<Users> GetUserById(Guid userId)
    {
       var user = await _usersRepository.GetUserById(userId) ?? throw new Exception("User not found");

        return user;
    }

    public async Task<Users> CreateUser(Users user)
    {
        var existingUser = await _usersRepository.GetUserByEmail(user.Email);

        if (existingUser != null) throw new Exception("Email already exists!");

        user.Password = PasswordHasherHelper.HashPassword(user.Password);

        return await _usersRepository.CreateUser(user);
    }

    public async Task<Users> UpdateUser(Guid userId, Users updateUser)
    {
        var currentUser = await GetUserById(userId);

        currentUser.Name = updateUser.Name;
        currentUser.Email = updateUser.Email;
        currentUser.Password = PasswordHasherHelper.HashPassword(updateUser.Password);

        await _usersRepository.UpdateUser(currentUser);
        return currentUser;
    }

    public async Task DeleteUser(Guid userId)
    {
        await _usersRepository.DeleteUser(userId);
    }
}
