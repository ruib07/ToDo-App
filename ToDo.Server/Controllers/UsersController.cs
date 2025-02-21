using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Server.Models;
using ToDo.Server.Services;

namespace ToDo.Server.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }

    // GET api/v1/users/{userId}
    [Authorize(Policy = Constants.Constants.PolicyUser)]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
    {
        var user = await _usersService.GetUserById(userId);

        return Ok(user);
    }

    // PUT api/v1/users/{userId}
    [Authorize(Policy = Constants.Constants.PolicyUser)]
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] Users updateUser)
    {
        await _usersService.UpdateUser(userId, updateUser);

        return Ok("User updated successfully.");
    }

    // DELETE api/v1/users/{userId}
    [Authorize(Policy = Constants.Constants.PolicyUser)]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        await _usersService.DeleteUser(userId);

        return NoContent();
    }
}
