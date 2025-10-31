using FeedApp.Application;
using FeedApp.Application.Contracts.Identity;
using FeedApp.Blazor.Common.Contracts;
using FeedApp.BlazorServer.Components;
using AttrectoTest.BlazorServer.Services.IamBase;
using FeedApp.Infrastructure;
using FeedApp.Persistence;

using Blazored.SessionStorage;

using FeedApp.BlazorServer.Configuration;
using FeedApp.BlazorServer.Providers;
using FeedApp.BlazorServer.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddAppAuthServices(builder.Configuration);

builder.Services.AddMemoryCache();


builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IFeedService, FeedService>();

var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

if (apiSettings != null) { 
    builder.Services.AddHttpClient<IAuthClient, AuthClient>(client => client.BaseAddress = new Uri(apiSettings.IamBaseUrl));
    builder.Services.AddSingleton<IIamService>(new IamGrpcService(apiSettings.IamBaseUrl));
}

builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();
    var client = new HttpClient { BaseAddress = new Uri(nav.BaseUri),
        DefaultRequestHeaders = { { "Accept", "application/json" } } };
    return client;
});

builder.Services.AddControllers();
var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthentication();

app.MapControllers();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//app.Services.RunDatabaseMigrations();

app.Run();
