using AttrectoTest.ApiService.Jobs;

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
}
