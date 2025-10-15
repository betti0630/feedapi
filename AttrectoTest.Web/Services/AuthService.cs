using AttrectoTest.Web.Providers;
using AttrectoTest.Web.Services.Base;
using AttrectoTest.Web.Shared.Contracts;

using Blazored.SessionStorage;

using Microsoft.AspNetCore.Components.Authorization;

namespace AttrectoTest.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthClient _authClient;
        private readonly ISessionStorageService _sessionStorage;
        private readonly AuthenticationStateProvider _authStateProvider;


        public AuthService(IAuthClient authClient, ISessionStorageService sessionStorage, AuthenticationStateProvider authStateProvider)
        {
            _authClient = authClient;
            _sessionStorage = sessionStorage;
            _authStateProvider = authStateProvider;
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
                await _sessionStorage.SetItemAsync("jwt", jwtToken);
                ((JwtAuthStateProvider)_authStateProvider).NotifyUserAuthentication(jwtToken);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task Logout()
        {
            await ((JwtAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }
    }
}
