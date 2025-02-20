using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ToDo.Server.Data;
using ToDo.Server.Models;
using ToDo.Server.Models.DTOs;
using ToDo.Server.Services;

namespace ToDo.Server.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ToDoDbContext _context;
    private readonly UsersService _usersService;
    private readonly Jwt _jwtSettings;

    public AuthenticationController(ToDoDbContext context, UsersService usersService, Jwt jwtSettings)
    {
        _context = context;
        _usersService = usersService;
        _jwtSettings = jwtSettings;
    }

    // POST api/v1/auth/signin
    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<IActionResult> Signin([FromBody, Required] LoginRequest loginRequest)
    {
        if (loginRequest == null) return BadRequest("Email and password are mandatory.");
        if (string.IsNullOrWhiteSpace(loginRequest.Email)) return BadRequest("Email is required.");
        if (string.IsNullOrWhiteSpace(loginRequest.Password)) return BadRequest("Password is required.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

        if (user == null) return Unauthorized("User not found.");
        if (!VerifyPassword(loginRequest.Password, user.Password)) return Unauthorized("Incorrect password!");

        var claims = new List<Claim>()
        {
            new("Id", user.Id.ToString()),
            new(ClaimTypes.Role, "User"),
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new("Name", user.Name),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return Ok(new LoginResponse(jwtToken));
    }

    // POST api/v1/auth/signup
    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody, Required] SignupRequest signupRequest)
    {
        if (signupRequest == null) return BadRequest("All fields are required.");
        if (string.IsNullOrWhiteSpace(signupRequest.Email) || string.IsNullOrWhiteSpace(signupRequest.Password) || 
            string.IsNullOrWhiteSpace(signupRequest.Name)) return BadRequest("Invalid input.");

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == signupRequest.Email);
        if (existingUser != null) return BadRequest("Email already in use.");

        var user = new Users()
        {
            Id = Guid.NewGuid(),
            Email = signupRequest.Email,
            Password = signupRequest.Password,
            Name = signupRequest.Name
        };

        var createdUser = await _usersService.CreateUser(user);

        return CreatedAtAction(nameof(Signin), new { email = createdUser.Email }, new { message = "User created successfully!" });
    }

    #region Private Methods

    private static bool VerifyPassword(string providedPassword, string storedHash)
    {
        byte[] hashBytes = Convert.FromBase64String(storedHash);
        byte[] salt = hashBytes[..16];

        using var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        return hash.SequenceEqual(hashBytes[16..]);
    }

    #endregion
}
