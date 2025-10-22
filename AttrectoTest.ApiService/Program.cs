using AttrectoTest.ApiService.Configuration;
using AttrectoTest.ApiService.Helpers;
using AttrectoTest.ApiService.Middleware;
using AttrectoTest.ApiService.Services;
using AttrectoTest.ApiService.Validators;
using AttrectoTest.Application;
using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Infrastructure;
using AttrectoTest.Persistence;

using FluentValidation;
using FluentValidation.AspNetCore;

using Serilog;

using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .ReadFrom.Configuration(context.Configuration));


// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddScoped<IImageFileProcessor, ImageFileProcessor>();

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
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

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

builder.Services.ConfigureQuartz(builder.Configuration);

builder.Services.ConfigureHealthCheck(builder.Configuration);

var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

if (apiSettings != null && Uri.TryCreate(apiSettings.IamBaseUrl, new UriCreationOptions(), out Uri? iamBaseUrl)) { 
    builder.Services.AddSingleton<IIamService>(new IamGrpcService(iamBaseUrl));
}

var app = builder.Build();


app.UseCors("web");
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
//app.UseExceptionHandler();

app.MapDefaultEndpoints();


if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
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



