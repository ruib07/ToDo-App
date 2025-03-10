using Moq;
using ToDo.Server.Models;
using ToDo.Server.Repositories.Interfaces;
using ToDo.Server.Services;
using ToDo.Tests.Templates;

namespace ToDo.Tests.Services;

[TestFixture]
public class UsersServiceTests
{
    private Mock<IUsersRepository> _usersRepositoryMock;
    private UsersService _usersService;

    [SetUp]
    public void Setup()
    {
        _usersRepositoryMock = new Mock<IUsersRepository>();
        _usersService = new UsersService(_usersRepositoryMock.Object);
    }

    [Test]
    public async Task GetUserById_ReturnsUser()
    {
        var user = UsersTests.CreateUser();

        _usersRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync(user);

        var result = await _usersService.GetUserById(user.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(user.Id));
            Assert.That(result.Name, Is.EqualTo(user.Name));
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.Password, Is.EqualTo(user.Password));
        });
    }

    [Test]
    public void GetUserById_ReturnsNotFound_WhenUserNotFound()
    {
        _usersRepositoryMock.Setup(repo => repo.GetUserById(It.IsAny<Guid>())).ReturnsAsync((Users)null);

        var exception = Assert.ThrowsAsync<Exception>(async () => await _usersService.GetUserById(Guid.NewGuid()));

        Assert.That(exception.Message, Is.EqualTo("User not found"));
    }

    [Test]
    public async Task CreateUser_CreatesSuccessfully()
    {
        var user = UsersTests.CreateUser();

        _usersRepositoryMock.Setup(repo => repo.CreateUser(user)).ReturnsAsync(user);

        var result = await _usersService.CreateUser(user);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(user.Id));
            Assert.That(result.Name, Is.EqualTo(user.Name));
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.Password, Is.EqualTo(user.Password));
        });
    }

    [Test]
    public void CreateUser_ReturnsConflict_WhenEmailAlreadyExists()
    {
        var user = UsersTests.CreateUser();

        _usersRepositoryMock.Setup(repo => repo.GetUserByEmail(user.Email)).ReturnsAsync(user);

        var exception = Assert.ThrowsAsync<Exception>(async () => await _usersService.CreateUser(user));

        Assert.That(exception.Message, Is.EqualTo("Email already exists!"));
    }

    [Test]
    public async Task UpdateUser_UpdatesSuccessfully()
    {
        var user = UsersTests.CreateUser();
        var updateUser = UsersTests.UpdateUser(user.Id);

        _usersRepositoryMock.Setup(repo => repo.CreateUser(user)).ReturnsAsync(user);
        _usersRepositoryMock.Setup(repo => repo.UpdateUser(updateUser)).Returns(Task.CompletedTask);
        _usersRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync(user);

        await _usersService.UpdateUser(user.Id, updateUser);
        var result = await _usersService.GetUserById(user.Id);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(updateUser.Id));
            Assert.That(result.Name, Is.EqualTo(updateUser.Name));
            Assert.That(result.Email, Is.EqualTo(updateUser.Email));
        });
    }

    [Test]
    public async Task DeleteUser_DeletesSuccessfully()
    {
        var user = UsersTests.CreateUser();

        _usersRepositoryMock.Setup(repo => repo.CreateUser(user)).ReturnsAsync(user);
        _usersRepositoryMock.Setup(repo => repo.DeleteUser(user.Id)).Returns(Task.CompletedTask);
        _usersRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync((Users)null);

        await _usersService.CreateUser(user);
        await _usersService.DeleteUser(user.Id);

        var exception = Assert.ThrowsAsync<Exception>(async () => await _usersService.GetUserById(user.Id));

        Assert.That(exception.Message, Is.EqualTo("User not found"));
    }
}
