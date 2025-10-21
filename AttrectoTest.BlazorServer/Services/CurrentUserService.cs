using AttrectoTest.Application.Contracts.Identity;

using Microsoft.AspNetCore.Components.Authorization;

using System.Security.Claims;

namespace AttrectoTest.BlazorServer.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly AuthenticationStateProvider _authStateProvider;

    public CurrentUserService(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    public ClaimsPrincipal? User
    {
        get
        {
            // aszinkron hívásból lehet, hogy Task<AuthenticationState>-et kell await-elni
            // vagy az InitializeAsync után már _token-ból generált identity-t használni
            var authState = _authStateProvider.GetAuthenticationStateAsync().Result;
            return authState.User;
        }
    }
}
