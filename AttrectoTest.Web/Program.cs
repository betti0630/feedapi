using AttrectoTest.Web;
using AttrectoTest.Web.Auth;
using AttrectoTest.Web.Components;
using AttrectoTest.Web.Providers;

using Blazored.LocalStorage;

using HR.LeaveManagement.BlazorUI.Handlers;
using HR.LeaveManagement.BlazorUI.Providers;

using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddHttpContextAccessor();

// Cookie alapú auth
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// Saját provider regisztrálása
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddScoped<TokenHandler>();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://apiservice";

//builder.Services.AddTransient<JwtAuthorizationMessageHandler>();
builder.Services.AddHttpClient<WeatherApiClient>("api", client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new(apiBaseUrl);
    })
    .AddHttpMessageHandler<TokenHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
