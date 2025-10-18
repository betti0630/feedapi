using AttrectoTest.Aim.Persistence;
using AttrectoTest.Aim.Application;
using AttrectoTest.Aim.Infrastructure;
using Microsoft.AspNetCore.Builder;

using Serilog;
using AttrectoTest.AimService.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
    .WriteTo.Console()
    .ReadFrom.Configuration(context.Configuration));

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();


builder.Services.AddMemoryCache();

// Add services to the container.
builder.Services.AddProblemDetails();

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

if (allowedOrigins != null && allowedOrigins.Length > 0)
{
    builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("web", p => p
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod());
    });
}

builder.Services.AddAppAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddControllers();
//builder.Services.AddFastEndpoints();

builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "AttrectoTest API";
    config.Version = "v1";

    // Security Definition (Bearer JWT)
    config.AddSecurity("JWT", new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Type into the textbox: Bearer {your JWT token}."
    });

    // Security Requirement (apply globally)
    config.OperationProcessors.Add(
        new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

var app = builder.Build();


app.UseCors("web");
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
//app.UseExceptionHandler();



if (app.Environment.IsDevelopment())
{
    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    app.UseOpenApi();

    // Add web UIs to interact with the document
    // Available at: http://localhost:<port>/swagger
    app.UseSwaggerUi();

    // Add ReDoc UI to interact with the document
    // Available at: http://localhost:<port>/redoc
    app.UseReDoc(options =>
    {
        options.Path = "/redoc";
    });
}
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseStaticFiles();

app.Services.RunDatabaseMigrations();

app.Run();
