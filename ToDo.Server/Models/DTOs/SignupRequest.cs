namespace ToDo.Server.Models.DTOs;

public class SignupRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
}
