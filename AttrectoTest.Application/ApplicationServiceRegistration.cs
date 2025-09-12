using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Identity;
using AttrectoTest.Domain;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AttrectoTest.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
        services.AddScoped<IAppUserService, AppUserService>();
        return services;
    }
}
