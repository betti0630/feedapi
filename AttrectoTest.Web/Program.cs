using AttrectoTest.Web;
using AttrectoTest.Web.Helpers;

using Blazored.SessionStorage;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5481/") });
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();

builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
