using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Blazor.Shared.Contracts;
using AttrectoTest.BlazorServer.Providers;
using AttrectoTest.BlazorServer.Services.IamBase;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using Newtonsoft.Json.Linq;

using System.Security.Claims;



namespace AttrecotTest.BlazorServer.Services;

public class AuthManager : IAuthManager
{
    private readonly IAuthClient _authClient;
    private readonly NavigationManager _nav;
    private readonly IHttpContextAccessor _httpContext;
    private readonly AuthenticationStateProvider _authProvider;


    public AuthManager(IAuthClient authClient,
        IHttpContextAccessor httpContextAccessor, HttpClient httpClient, NavigationManager nav,
        AuthenticationStateProvider authProvider, IHttpContextAccessor httpContext)
    {

        _authClient = authClient;
        _nav = nav;
        _httpContext = httpContext;
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
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    public async Task Logout()
    {
        await ((CustomAuthStateProvider)_authProvider).MarkUserAsLoggedOut();
        _nav.NavigateTo("/", forceLoad: true);
    }
}
