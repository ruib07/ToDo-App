namespace ToDo.Server.Models.DTOs;

public class LoginResponse
{
    public LoginResponse()
    {
        TokenType = "Bearer";
    }

    public LoginResponse(string accessToken) : this()
    {
        AccessToken = accessToken;
    }

    public string AccessToken { get; set; }
    public string TokenType { get; set; }
}
