using AttrectoTest.ApiService.Middleware;
using AttrectoTest.ApiService.Validators;
using AttrectoTest.Application;
using AttrectoTest.Infrastructure;
using AttrectoTest.Persistence;

using FluentValidation;
using FluentValidation.AspNetCore;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
    .WriteTo.Console()
    .ReadFrom.Configuration(context.Configuration));


// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddScoped<IImageFileProcessor, ImageFileProcessor>();

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

var app = builder.Build();



app.Services.RunDatabaseMigrations();

app.UseCors("wasm");
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
//app.UseExceptionHandler();

app.MapDefaultEndpoints();

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

app.Run();



