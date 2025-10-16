using AttrectoTest.BlazorWasm;
using AttrectoTest.Blazor.Shared.Contracts;
using AttrectoTest.BlazorWasm.Handlers;
using AttrectoTest.BlazorWasm.Providers;
using AttrectoTest.BlazorWasm.Services;
using AttrectoTest.BlazorWasm.Services.Base;

using Blazored.SessionStorage;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddTransient<JwtAuthorizationMessageHandler>();
builder.Services.AddHttpClient<IAuthClient, AuthClient>(client => client.BaseAddress = new Uri("http://localhost:5481/"))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();
builder.Services.AddHttpClient<IFeedsClient, FeedsClient>( client => client.BaseAddress = new Uri("http://localhost:5481/"))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFeedService, FeedService>();

builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();

builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
