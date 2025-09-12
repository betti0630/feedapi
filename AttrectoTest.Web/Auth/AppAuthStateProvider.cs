using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Authorization;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AttrectoTest.Web.Auth;

public class AppAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    private string? _cachedToken;

    public AppAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = _cachedToken ?? await _localStorage.GetItemAsync<string>("auth_token");

        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        try
        {
            var jwt = _tokenHandler.ReadJwtToken(token);

            if (jwt.ValidTo < DateTime.UtcNow)
            {
                await _localStorage.RemoveItemAsync("auth_token");
                _cachedToken = null;
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _cachedToken = token; // cache
            var identity = new ClaimsIdentity(jwt.Claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            await _localStorage.RemoveItemAsync("auth_token");
            _cachedToken = null;
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _localStorage.SetItemAsync("auth_token", token);
        _cachedToken = token;

        var jwt = _tokenHandler.ReadJwtToken(token);
        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveItemAsync("auth_token");
        _cachedToken = null;

        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }

    // Új: Token lekérése biztonságosan
    public string? GetToken() => _cachedToken;
}