using AttrectoTest.Iam.Application.Contracts.Logging;
using AttrectoTest.Iam.Application.Contracts.Notification;
using AttrectoTest.Iam.Infrastructure.Logging;
using AttrectoTest.Iam.Infrastructure.Model;
using AttrectoTest.Iam.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace AttrectoTest.Iam.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

        var apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>();
        services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));

        if (apiSettings != null && Uri.TryCreate(apiSettings.NotificationUrl, new UriCreationOptions(), out Uri? iamBaseUrl))
        {
            services.AddSingleton<INotificationService>(new NotificationService(iamBaseUrl));
        }

        return services;
    }
}
