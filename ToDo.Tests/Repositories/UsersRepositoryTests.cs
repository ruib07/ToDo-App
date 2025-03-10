using Microsoft.EntityFrameworkCore;
using ToDo.Server.Data;
using ToDo.Server.Repositories;
using ToDo.Tests.Templates;

namespace ToDo.Tests.Repositories;

[TestFixture]
public class UsersRepositoryTests
{
    private UsersRepository _usersRepository;
    private ToDoDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ToDoDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ToDoDbContext(options);
        _usersRepository = new UsersRepository(_context);
    }

    [Test]
    public async Task GetUserById_ReturnsUser()
    {
        var user = UsersTests.CreateUser();

        await _usersRepository.CreateUser(user);

        var result = await _usersRepository.GetUserById(user.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(user.Id));
            Assert.That(result.Name, Is.EqualTo(user.Name));
            Assert.That(result.Email, Is.EqualTo(user.Email));
        });
    }

    [Test]
    public async Task GetUserByEmail_ReturnsUser()
    {
        var user = UsersTests.CreateUser();

        await _usersRepository.CreateUser(user);

        var result = await _usersRepository.GetUserByEmail(user.Email);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(user.Id));
            Assert.That(result.Name, Is.EqualTo(user.Name));
            Assert.That(result.Email, Is.EqualTo(user.Email));
        });
    }

    [Test]
    public async Task CreateUser_CreatesSuccessfully()
    {
        var newUser = UsersTests.CreateUser();

        var result = await _usersRepository.CreateUser(newUser);
        var addedUser = await _usersRepository.GetUserById(newUser.Id);

        Assert.That(addedUser, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedUser.Id, Is.EqualTo(newUser.Id));
            Assert.That(addedUser.Name, Is.EqualTo(newUser.Name));
            Assert.That(addedUser.Email, Is.EqualTo(newUser.Email));
        });
    }

    [Test]
    public async Task UpdateUser_UpdatesSuccessfully()
    {
        var existingUser = UsersTests.CreateUser();
        await _usersRepository.CreateUser(existingUser);

        _context.Entry(existingUser).State = EntityState.Detached;

        var updatedUser = UsersTests.UpdateUser(existingUser.Id);

        await _usersRepository.UpdateUser(updatedUser);
        var retrievedUpdatedUser = await _usersRepository.GetUserById(existingUser.Id);

        Assert.That(retrievedUpdatedUser, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(retrievedUpdatedUser.Name, Is.EqualTo(updatedUser.Name));
            Assert.That(retrievedUpdatedUser.Email, Is.EqualTo(updatedUser.Email));
        });
    }

    [Test]
    public async Task DeleteUser_DeletesSuccessfully()
    {
        var existingUser = UsersTests.CreateUser();

        await _usersRepository.CreateUser(existingUser);
        await _usersRepository.DeleteUser(existingUser.Id);
        var retrivedEmptyAdmin = await _usersRepository.GetUserById(existingUser.Id);

        Assert.That(retrivedEmptyAdmin, Is.Null);
    }
}
