using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Backend.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<IdentityUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> RegisterAsync(string email, string username, string password, string role)
    {
        var user = new IdentityUser { UserName = username, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return FailedRegistration(result, email, username);
        }

        await _userManager.AddToRoleAsync(user, role); // Adding the user to a role
        
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.CreateToken(user, roles[0]);
        return new AuthResult(true, username, accessToken);
    }


    private static AuthResult FailedRegistration(IdentityResult result, string email, string username)
    {
        var authResult = new AuthResult(false, username, "");

        foreach (var error in result.Errors)
        {
            authResult.ErrorMessages.Add(error.Code, error.Description);
        }

        return authResult;
    }
    
    
    public async Task<AuthResult> LoginAsync(string userName, string password)
    {
        var managedUser = await _userManager.FindByNameAsync(userName);

        if (managedUser == null)
        {
            return InvalidUsernameOrPassword();
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
        if (!isPasswordValid)
        {
            return InvalidUsernameOrPassword();
        }


        // get the role and pass it to the TokenService
        var roles = await _userManager.GetRolesAsync(managedUser);
        var accessToken = _tokenService.CreateToken(managedUser, roles[0]);

        return new AuthResult(true, managedUser.UserName!, accessToken);
    }
    

    private static AuthResult InvalidUsernameOrPassword()
    {
        var result = new AuthResult(false, "", "");
        result.ErrorMessages.Add("Bad credentials", "Invalid username or password");
        return result;
    }

}
