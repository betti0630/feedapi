using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

using System.Security.Claims;

namespace AttrectoTest.Web.Providers;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomAuthStateProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var result = await _httpContextAccessor.HttpContext!.AuthenticateAsync("Cookies");

        if (result?.Principal != null && result.Principal.Identity?.IsAuthenticated == true)
        {
            return new AuthenticationState(result.Principal);
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task LoginAsync(string username, string jwtToken)
    {
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, username)
    };

        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);

        var props = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        props.StoreTokens(new[]
        {
        new AuthenticationToken { Name = "access_token", Value = jwtToken }
    });

        if (_httpContextAccessor.HttpContext.User.Identity?.IsAuthenticated == true)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync("Cookies");
        }
        try {
            await _httpContextAccessor.HttpContext!.SignInAsync("Cookies", principal, props);
        }
        catch (Exception ex) {
            //TODO: Why?
        }

        // jelezni a Blazor keretrendszernek, hogy a state változott
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public async Task LogoutAsync()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync("Cookies");

        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        var result = await _httpContextAccessor.HttpContext!.AuthenticateAsync("Cookies");
        return result.Properties?.GetTokenValue("access_token");
    }
}
