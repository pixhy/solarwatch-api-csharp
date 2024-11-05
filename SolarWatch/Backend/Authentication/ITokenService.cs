using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Backend.Authentication;

public interface ITokenService
{
    string CreateToken(IdentityUser user, string role);
}