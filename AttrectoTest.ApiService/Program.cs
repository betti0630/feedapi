using AttrectoTest.Application;
using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Persistence;
using AttrectoTest.Infrastructure;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
    .WriteTo.Console()
    .ReadFrom.Configuration(context.Configuration));


// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Add services to the container.
builder.Services.AddProblemDetails();


builder.Services.AddCors(opt =>
{
    opt.AddPolicy("wasm", p => p
        .WithOrigins("http://webfrontend:80", "http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddAppAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

app.Services.RunDatabaseMigrations();

app.UseCors("wasm");
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();



app.MapPost("/auth/login", async (LoginRequest req, IAuthService authService) =>
{
    if (!await authService.ValidateUser(req.UserName, req.Password))
        return Results.Unauthorized();
    var tokenResult = await authService.GenerateJwtToken(req.UserName);
    return Results.Ok(new LoginResponse(tokenResult.token, tokenResult.expires));
});

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.RequireAuthorization();

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record LoginRequest(string UserName, string Password);
record LoginResponse(string Token, DateTime ExpiresAt);