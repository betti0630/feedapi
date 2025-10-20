using AttrectoTest.ApiService.Jobs;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

using Quartz;

namespace AttrectoTest.ApiService.Helpers;

public static class ConfigurationHelper
{
    public static IServiceCollection ConfigureQuartz(this IServiceCollection services, IConfiguration configuration)
    {
        // base configuration from appsettings.json
        services.Configure<QuartzOptions>(configuration.GetSection("Quartz"));

        services.AddQuartz(q =>
        {

            var jobKey = new JobKey("PurgeFeedsJob");
            var cron = configuration["Quartz:Jobs:PurgeFeedsJob:Cron"];
            var tzId = configuration["Quartz:Jobs:PurgeFeedsJob:TimeZone"];
            var tz = !string.IsNullOrEmpty(tzId)
            ? TimeZoneInfo.FindSystemTimeZoneById(tzId)
            : TimeZoneInfo.Local;

            q.AddJob<PurgeFeedsJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("PurgeFeedsJob-Daily")
                .WithCronSchedule(cron ?? "0 0 0 * * ?", x => x.InTimeZone(tz))
            );
        });


        // Quartz.Extensions.Hosting allows you to fire background service that handles scheduler lifecycle
        services.AddQuartzHostedService(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });

        return services;
    }

    public static IServiceCollection ConfigureHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
           .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        services.AddHealthChecks()
            .AddMySql(
                connectionString: configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("DefaultConnection is not configured."),
                timeout: TimeSpan.FromSeconds(5),
                name: "mariadb",
                tags: new[] { "mariadb" });

        services.AddHealthChecks()
        .AddCheck<AimApiHealthCheck>("aim_api_grpc", tags: new[] { "aimapi" });

        var urls = configuration["ASPNETCORE_URLS"] ?? "http://localhost:5000";

        var firstUrl = urls.Split(';', StringSplitOptions.RemoveEmptyEntries).First().Trim();

        var safeUrl = firstUrl.Replace("0.0.0.0", "localhost");

        //services
        //    .AddHealthChecksUI(opt =>
        //    {
        //        opt.SetEvaluationTimeInSeconds(30); 
        //        opt.MaximumHistoryEntriesPerEndpoint(50);
        //        var safeUrl = urls.TrimEnd('/');
        //        opt.AddHealthCheckEndpoint("FeedAPI", $"{safeUrl}/health");
        //    })
        //    .AddInMemoryStorage();

        return services;
    }

    private const string HealthEndpointPath = "/health";
    private const string AlivenessEndpointPath = "/alive";

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        // All health checks must pass for app to be considered ready to accept traffic after starting
        app.MapHealthChecks(HealthEndpointPath, new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Only health checks tagged with the "live" tag must pass for app to be considered alive
        app.MapHealthChecks(AlivenessEndpointPath, new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live")
        });

        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.MapHealthChecksUI(options =>
            {
                options.UIPath = "/health-ui";
            });
        }

        return app;
    }

    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.ConfigureOpenTelemetry();


        //builder.Services.AddServiceDiscovery();

        //builder.Services.ConfigureHttpClientDefaults(http =>
        //{
        //    // Turn on resilience by default
        //    http.AddStandardResilienceHandler();

        //    // Turn on service discovery by default
        //    http.AddServiceDiscovery();
        //});

        // Uncomment the following to restrict the allowed schemes for service discovery.
        // builder.Services.Configure<ServiceDiscoveryOptions>(options =>
        // {
        //     options.AllowedSchemes = ["https"];
        // });

        return builder;
    }

    public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing.AddSource(builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation(tracing =>
                        // Exclude health check requests from tracing
                        tracing.Filter = context =>
                            !context.Request.Path.StartsWithSegments(HealthEndpointPath)
                            && !context.Request.Path.StartsWithSegments(AlivenessEndpointPath)
                    )
                    // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
                    //.AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
        //if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        //{
        //    builder.Services.AddOpenTelemetry()
        //       .UseAzureMonitor();
        //}

        return builder;
    }

}
