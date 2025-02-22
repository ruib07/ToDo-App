namespace ToDo.Server.Models.DTOs;

public record LoginResponse(string AccessToken, string TokenType = "Bearer");
