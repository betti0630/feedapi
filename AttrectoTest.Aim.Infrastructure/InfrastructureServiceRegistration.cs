using AttrectoTest.Aim.Infrastructure.Logging;
using AttrectoTest.Aim.Application.Contracts.Logging;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace AttrectoTest.Aim.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        return services;
    }
}
