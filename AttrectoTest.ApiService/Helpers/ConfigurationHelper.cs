using AttrectoTest.ApiService.Jobs;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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

        services
            .AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(30); 
                opt.MaximumHistoryEntriesPerEndpoint(50);
                opt.AddHealthCheckEndpoint("DB", "/health");
            })
            .AddInMemoryStorage();

        return services;
    }

    private const string HealthEndpointPath = "/health";
    private const string AlivenessEndpointPath = "/alive";

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        if (app.Environment.IsDevelopment())
        {
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

            app.MapHealthChecksUI(options =>
            {
                options.UIPath = "/health-ui"; 
            });
        }

        return app;
    }
}
