namespace SolarWatch.Backend.Authentication;

public record AuthResult(
    bool Success,
    string UserName,
    string Token)
{
    //Error code - error message
    public readonly Dictionary<string, string> ErrorMessages = new();
}
