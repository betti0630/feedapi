using AttrectoTest.Blazor.Shared.Contracts;
using AttrectoTest.BlazorServer.Client.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;

using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AttrectoTest.BlazorServer.Client.Pages
{
    public partial class Login
    {
        [SupplyParameterFromForm]
        public LoginModel LoginReq { get; set; } = new LoginModel();

        private string? _error;

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        [Inject] protected IAuthManager AuthManager { get; set; } = null!;
        [Inject] protected HttpClient _http { get; set;}


        public async void LoginTask()
        {
            var ok = await AuthManager.Login(LoginReq.UserName, LoginReq.Password);
            if (ok)
            {
                var userId = LoginReq.UserName switch
                {
                    "alice" => 1,
                    "bob" => 2,
                    _ => 0
                };
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, LoginReq.UserName),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                HttpContext.Response.Redirect("/");
            }
            else
            {
                _error = "Login failed";
            }

        }

    }
}
