using AttrectoTest.Notification.Infrastructure;
using AttrectoTest.Notification.Infrastructure.Models;
using AttrectoTest.NotificationService.Services;

using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<NotificationServiceImpl>();
app.MapGet("/", () => "Notification gRPC service");
app.Run();
