using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AttrectoTest.Iam.Infrastructure.Logging;
using AttrectoTest.Iam.Application.Contracts.Logging;


namespace AttrectoTest.Iam.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        return services;
    }
}
