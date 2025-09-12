var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AttrectoTest_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");


builder.Build().Run();
