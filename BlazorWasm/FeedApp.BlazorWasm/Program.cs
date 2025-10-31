using AttrectoTest.BlazorWasm;
using AttrectoTest.BlazorWasm.Services.IamBase;
using AttrectoTest.BlazorWasm.Services.Base;

using Blazored.SessionStorage;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using System.Reflection;
using FeedApp.Blazor.Common.Contracts;
using FeedApp.BlazorWasm.Providers;
using FeedApp.BlazorWasm.Handlers;
using FeedApp.BlazorWasm.Services;
using FeedApp.BlazorWasm.Configuration;
using FeedApp.BlazorWasm;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

if (apiSettings != null) { 
    builder.Services.AddHttpClient<IAuthClient, AuthClient>(client => client.BaseAddress = new Uri(apiSettings.IamBaseUrl))
        .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();
    builder.Services.AddHttpClient<IFeedsClient, FeedsClient>( client => client.BaseAddress = new Uri(apiSettings.BaseUrl))
        .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();
}

builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IFeedService, FeedService>();

builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();

builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
