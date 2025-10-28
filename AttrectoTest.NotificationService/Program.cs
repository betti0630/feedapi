using AttrectoTest.NotificationService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<NotificationServiceImpl>();
app.MapGet("/", () => "Notification gRPC service");
app.Run();
