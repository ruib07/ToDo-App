namespace ToDo.Server.Models.DTOs;

public record Jwt(string Issuer, string Audience, string Key);