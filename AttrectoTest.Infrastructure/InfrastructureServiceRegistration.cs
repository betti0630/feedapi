using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Infrastructure.Logging;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace AttrectoTest.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        return services;
    }
}
