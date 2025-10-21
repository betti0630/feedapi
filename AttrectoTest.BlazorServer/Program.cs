using AttrecotTest.BlazorServer.Services;

using AttrectoTest.Application;
using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Blazor.Shared.Contracts;
using AttrectoTest.BlazorServer.Components;
using AttrectoTest.BlazorServer.Configuration;
using AttrectoTest.BlazorServer.Providers;
using AttrectoTest.BlazorServer.Services;
using AttrectoTest.BlazorServer.Services.IamBase;
using AttrectoTest.Infrastructure;
using AttrectoTest.Persistence;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
//builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddAppAuthServices(builder.Configuration);

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IFeedService, FeedService>();

var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));



builder.Services.AddHttpClient<IAuthClient, AuthClient>(client => client.BaseAddress = new Uri(apiSettings.IamBaseUrl));
// .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

builder.Services.AddSingleton<IIamService>(new IamGrpcService(apiSettings.IamBaseUrl));

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

app.Services.RunDatabaseMigrations();

app.Run();
