using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
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

        user.Password = HashPassword(user.Password);

        return await _usersRepository.CreateUser(user);
    }

    public async Task<Users> UpdateUser(Guid userId, Users updateUser)
    {
        var currentUser = await GetUserById(userId);

        currentUser.Name = updateUser.Name;
        currentUser.Email = updateUser.Email;
        currentUser.Password = HashPassword(updateUser.Password);

        await _usersRepository.UpdateUser(currentUser);
        return currentUser;
    }

    public async Task DeleteUser(Guid userId)
    {
        await _usersRepository.DeleteUser(userId);
    }

    #region Private Methods

    private static string HashPassword(string password)
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32);

        byte[] hashBytes = new byte[16 + 32];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 32);
        return Convert.ToBase64String(hashBytes);
    }

    #endregion
}
