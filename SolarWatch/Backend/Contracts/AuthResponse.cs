namespace SolarWatch.Backend.Contracts;

public record AuthResponse(string Email, string UserName, string Token);