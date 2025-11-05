using AttrectoTest.BlazorWasm;
using AttrectoTest.BlazorWasm.Services.Base;

using Blazored.SessionStorage;

using FeedApp.Blazor.Common.Contracts;
using FeedApp.BlazorWasm;
using FeedApp.BlazorWasm.Configuration;
using FeedApp.BlazorWasm.Handlers;
using FeedApp.BlazorWasm.Providers;
using FeedApp.BlazorWasm.Services;
using FeedApp.BlazorWasm.Services.IamBase;
using FeedApp.BlazorWasm.Services.NotificationBase;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;

using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

if (apiSettings != null) { 
    builder.Services.AddHttpClient<IAuthClient, AuthClient>(client => client.BaseAddress = new Uri(apiSettings.IamApiUrl))
        .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();
    builder.Services.AddHttpClient<INotificationsClient, NotificationsClient>(client => client.BaseAddress = apiSettings.NotificationUrl)
        .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();
    builder.Services.AddHttpClient<IFeedsClient, FeedsClient>(client => client.BaseAddress = new Uri(apiSettings.FeedApiUrl))
        .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();
}

if (apiSettings != null && apiSettings.NotificationUrl != null)
{
    builder.Services.AddSingleton<IFeedNotificationService>(new FeedNotificationService(apiSettings.NotificationUrl));
}

builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IFeedService, FeedService>();

builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();

builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
