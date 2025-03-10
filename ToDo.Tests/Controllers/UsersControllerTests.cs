using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDo.Server.Controllers;
using ToDo.Server.Models;
using ToDo.Server.Repositories.Interfaces;
using ToDo.Server.Services;
using ToDo.Tests.Templates;

namespace ToDo.Tests.Controllers;

[TestFixture]
public class UsersControllerTests
{
    private Mock<IUsersRepository> _usersRepositoryMock;
    private UsersService _usersService;
    private UsersController _usersController;

    [SetUp]
    public void Setup()
    {
        _usersRepositoryMock = new Mock<IUsersRepository>();
        _usersService = new UsersService(_usersRepositoryMock.Object);
        _usersController = new UsersController(_usersService);
    }

    [Test]
    public async Task GetUserById_ReturnsOkResult_WithUser()
    {
        var user = UsersTests.CreateUser();

        _usersRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync(user);

        var result = await _usersController.GetUserById(user.Id);
        var okResult = result as OkObjectResult;
        var response = okResult.Value as Users;

        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(response.Id, Is.EqualTo(user.Id));
            Assert.That(response.Name, Is.EqualTo(user.Name));
            Assert.That(response.Email, Is.EqualTo(user.Email));
            Assert.That(response.Password, Is.EqualTo(user.Password));
        });
    }

    [Test]
    public async Task UpdateUser_ReturnsOkResult_WhenUserIsUpdated()
    {
        var user = UsersTests.CreateUser();
        var updatedUser = UsersTests.UpdateUser(user.Id);

        _usersRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync(user);
        _usersRepositoryMock.Setup(repo => repo.UpdateUser(It.IsAny<Users>())).Returns(Task.CompletedTask);

        var result = await _usersController.UpdateUser(user.Id, updatedUser);
        var okResult = result as OkObjectResult;

        Assert.That(okResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo("User updated successfully."));
        });
    }

    [Test]
    public async Task DeleteUser_ReturnsNoContent_WhenUserIsDeleted()
    {
        var user = UsersTests.CreateUser();

        _usersRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync(user);
        _usersRepositoryMock.Setup(repo => repo.DeleteUser(user.Id)).Returns(Task.CompletedTask);

        var result = await _usersController.DeleteUser(user.Id);
        var noContentResult = result as NoContentResult;

        Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
    }
}
