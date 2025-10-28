using AttrectoTest.Application;
using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Blazor.Common.Contracts;
using AttrectoTest.BlazorServer.Services.IamBase;
using AttrectoTest.BlazorWeb.Client.Pages;
using AttrectoTest.BlazorWeb.Components;
using AttrectoTest.BlazorWeb.Configuration;
using AttrectoTest.BlazorWeb.Handlers;
using AttrectoTest.BlazorWeb.Providers;
using AttrectoTest.BlazorWeb.Services;
using AttrectoTest.Infrastructure;
using AttrectoTest.Persistence;

using Blazored.SessionStorage;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services
    .AddAuthentication("DummyScheme")
    .AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>("DummyScheme", null);


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

if (apiSettings != null)
{
    builder.Services.AddHttpClient<IAuthClient, AuthClient>(client => client.BaseAddress = new Uri(apiSettings.IamBaseUrl));
    builder.Services.AddSingleton<IIamService>(new IamGrpcService(apiSettings.IamBaseUrl));
}

builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();
    var client = new HttpClient
    {
        BaseAddress = new Uri(nav.BaseUri),
        DefaultRequestHeaders = { { "Accept", "application/json" } }
    };
    return client;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(AttrectoTest.BlazorWeb.Client._Imports).Assembly,
        typeof(AttrectoTest.Blazor.Common.Components.Layout.TopBar).Assembly);

app.Run();
