using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Cryptography;
using ToDo.Server.Controllers;
using ToDo.Server.Data;
using ToDo.Server.Models;
using ToDo.Server.Models.DTOs;
using ToDo.Server.Repositories.Interfaces;
using ToDo.Server.Services;
using ToDo.Tests.Templates;

namespace ToDo.Tests.Controllers;

[TestFixture]
public class AuthenticationControllerTests
{
    private ToDoDbContext _context;
    private Jwt _jwt;
    private Mock<IUsersRepository> _userRepositoryMock;
    private UsersService _usersService;
    private AuthenticationController _authenticationController;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ToDoDbContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new ToDoDbContext(options);
        var key = GenerateRandomKey();
        _jwt = new Jwt("testIssuer", "testAudience", key);
        _userRepositoryMock = new Mock<IUsersRepository>();
        _usersService = new UsersService(_userRepositoryMock.Object);
        _authenticationController = new AuthenticationController(_context, _usersService, _jwt);
    }

    [Test]
    public async Task Signin_ReturnsOk_WhenCredentialsAreValid()
    {
        var user = UsersTests.CreateUser();
        _context.Users.Add(user); 
        await _context.SaveChangesAsync();

        var signinRequest = new LoginRequest(user.Email, "User@Test-123");

        var result = await _authenticationController.Signin(signinRequest);
        var okResult = result as OkObjectResult;
        var signinResponse = okResult.Value as LoginResponse;

        Assert.That(okResult, Is.Not.Null);
        Assert.That(signinResponse, Is.Not.Null);
        Assert.That(okResult.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public async Task Signin_ReturnsUnauthorized_WhenUserNotFound()
    {
        var signinRequest = new LoginRequest("user@email.com", "User@Test-123");

        var result = await _authenticationController.Signin(signinRequest);
        var unauthorizedResult = result as UnauthorizedObjectResult;

        Assert.That(unauthorizedResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(unauthorizedResult.StatusCode, Is.EqualTo(401));
            Assert.That(unauthorizedResult.Value, Is.EqualTo("User not found."));
        });
    }

    [Test]
    public async Task Signin_ReturnsBadRequest_WhenSigninRequestIsNull()
    {
        var result = await _authenticationController.Signin(null);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Email and password are mandatory."));
        });
    }

    [Test]
    public async Task Signin_ReturnsBadRequest_WhenEmailIsNull()
    {
        var signinRequest = new LoginRequest("", "Invalid@UserPassword-123");

        var result = await _authenticationController.Signin(signinRequest);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Email is required."));
        });
    }

    [Test]
    public async Task Signin_ReturnsBadRequest_WhenPasswordlIsNull()
    {
        var signinRequest = new LoginRequest("invaliduser@email.com", "");

        var result = await _authenticationController.Signin(signinRequest);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Password is required."));
        });
    }

    [Test]
    public async Task Signup_ReturnsCreatedResult_WhenFieldsAreValid()
    {
        var user = UsersTests.CreateUser();
        var signupRequest = new SignupRequest(user.Email, user.Password, user.Name);

        _userRepositoryMock.Setup(repo => repo.CreateUser(It.IsAny<Users>())).ReturnsAsync(user);

        var result = await _authenticationController.Signup(signupRequest);
        var createdResult = result as CreatedAtActionResult;

        Assert.That(createdResult.StatusCode, Is.EqualTo(201));
    }

    [Test]
    public async Task Signup_ReturnsBadRequest_WhenSignupRequestIsNull()
    {
        var result = await _authenticationController.Signup(null);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("All fields are required."));
        });
    }

    [Test]
    public async Task Signup_ReturnsBadRequest_WhenNameIsNull()
    {
        var user = UsersTests.CreateUser();
        var signupRequest = new SignupRequest(user.Email, user.Password, "");

        var result = await _authenticationController.Signup(signupRequest);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Invalid input."));
        });
    }

    [Test]
    public async Task Signup_ReturnsBadRequest_WhenEmailIsNull()
    {
        var user = UsersTests.CreateUser();
        var signupRequest = new SignupRequest("", user.Password, user.Name);

        var result = await _authenticationController.Signup(signupRequest);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Invalid input."));
        });
    }

    [Test]
    public async Task Signup_ReturnsBadRequest_WhenPasswordIsNull()
    {
        var user = UsersTests.CreateUser();
        var signupRequest = new SignupRequest("", user.Email, user.Name);

        var result = await _authenticationController.Signup(signupRequest);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Invalid input."));
        });
    }

    [Test]
    public async Task Signup_ReturnsBadRequest_WhenEmailAlreadyExists()
    {
        var existingUser = UsersTests.CreateUser();
        _context.Users.Add(existingUser);
        await _context.SaveChangesAsync();

        var signupRequest = new SignupRequest(existingUser.Email, "New@user-123", "New User");

        var result = await _authenticationController.Signup(signupRequest);
        var badRequestResult = result as BadRequestObjectResult;

        Assert.That(badRequestResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Email already in use."));
        });
    }

    #region Private Methods

    private static string GenerateRandomKey()
    {
        byte[] keyBytes = new byte[32];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }
        string base64Key = Convert.ToBase64String(keyBytes);

        base64Key = base64Key.Replace('_', '/').Replace('-', '+');

        return base64Key;
    }

    #endregion
}
