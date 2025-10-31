using FeedApp.Application;
using FeedApp.Application.Contracts.Identity;
using FeedApp.Blazor.Common.Contracts;
using FeedApp.BlazorWeb.Client.Pages;
using FeedApp.BlazorWeb.Components;
using FeedApp.BlazorWeb.Configuration;
using FeedApp.BlazorWeb.Handlers;
using FeedApp.BlazorWeb.Providers;
using FeedApp.BlazorWeb.Services;
using FeedApp.Infrastructure;
using FeedApp.Infrastructure.Services;
using FeedApp.Persistence;

using Blazored.SessionStorage;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System.Reflection;
using AttrectoTest.BlazorServer.Services.IamBase;

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
    if (Uri.TryCreate(apiSettings.IamBaseUrl, new UriCreationOptions(), out Uri? iamBaseUrl)) { 
        builder.Services.AddSingleton<IIamService>(new IamGrpcService(iamBaseUrl));
    }
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
        typeof(FeedApp.BlazorWeb.Client._Imports).Assembly,
        typeof(FeedApp.Blazor.Common.Components.Layout.TopBar).Assembly);

app.Run();
