using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Blazor.Shared.Contracts;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System.Security.Claims;



namespace AttrecotTest.BlazorServer.Services;

public class AuthManager : IAuthManager
{
    private readonly IAuthService _applicationAuthService;
    private readonly NavigationManager _nav;
    private readonly IHttpContextAccessor _httpContext;


    public AuthManager(IAuthService applicationAuthService,
        IHttpContextAccessor httpContextAccessor, HttpClient httpClient, NavigationManager nav,
        AuthenticationStateProvider authProvider, IHttpContextAccessor httpContext)
    {
       
        _applicationAuthService = applicationAuthService;
        _nav = nav;
        _httpContext = httpContext;
    }

    public async Task<bool> Login(string userName, string password)
    {
        return true;
        var result = await _applicationAuthService.ValidateUser(userName, password);
          if (result)
          {
            //var (token, expires) = await _applicationAuthService.GenerateJwtToken(userName);
            return true;
          }
          return false;
    }

    public async Task Logout()
    {
        _nav.NavigateTo("/", forceLoad: true);
    }
}
