using AttrectoTest.Blazor.Shared.Contracts;
using AttrectoTest.BlazorServer.Providers;
using AttrectoTest.BlazorServer.Services.IamBase;

using Microsoft.AspNetCore.Components.Authorization;

namespace AttrectoTest.BlazorServer.Services;

public class AuthManager : IAuthManager
{
    private readonly IAuthClient _authClient;
    private readonly AuthenticationStateProvider _authProvider;


    public AuthManager(IAuthClient authClient, AuthenticationStateProvider authProvider)
    {

        _authClient = authClient;
        _authProvider = authProvider;
    }

    public async Task<bool> Login(string userName, string password)
    {
        try
        {
            var request = new LoginRequest { UserName = userName, Password = password };
            var response = await _authClient.LoginAsync(request);

            if (response.Access_token == string.Empty)
            {
                return false;
            }

            var jwtToken = response.Access_token;
            await ((CustomAuthStateProvider)_authProvider).MarkUserAsAuthenticated(jwtToken);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public async Task Logout()
    {
        await ((CustomAuthStateProvider)_authProvider).MarkUserAsLoggedOut();

    }
}
