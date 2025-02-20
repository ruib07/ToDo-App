using Microsoft.EntityFrameworkCore;
using ToDo.Server.Data;
using ToDo.Server.Models;
using ToDo.Server.Repositories.Interfaces;

namespace ToDo.Server.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly ToDoDbContext _context;
    private DbSet<Users> Users => _context.Users;

    public UsersRepository(ToDoDbContext context)
    {
        _context = context;
    }

    public async Task<Users> GetUserById(Guid userId)
    {
        return await Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<Users> GetUserByEmail(string email)
    {
        return await Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Users> CreateUser(Users user)
    {
        await Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task UpdateUser(Users user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(Guid userId)
    {
        var deleteUser = await GetUserById(userId);

        Users.Remove(deleteUser);
        await _context.SaveChangesAsync();
    }
}
