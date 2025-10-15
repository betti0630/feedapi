using Blazored.SessionStorage;

using Microsoft.AspNetCore.Components.Authorization;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AttrectoTest.Web.Helpers
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ISessionStorageService _sessionStorage;
        private readonly AuthenticationStateProvider _authStateProvider;


        public AuthService(HttpClient http, ISessionStorageService sessionStorage, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _sessionStorage = sessionStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> LoginAsync(string userName, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", new { userName, password });

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            var jwtToken = result.GetProperty("access_token").GetString();


            await _sessionStorage.SetItemAsync( "jwt", jwtToken);

            ((JwtAuthStateProvider)_authStateProvider).NotifyUserAuthentication(jwtToken);

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            return true;
        }

        public void Logout()
        {
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }
}
