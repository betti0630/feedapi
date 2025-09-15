using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Features.Feed.Mappers;
using AttrectoTest.Application.Helpers;
using AttrectoTest.Application.Identity;
using AttrectoTest.Application.Models;
using AttrectoTest.Domain;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using System.Reflection;
using System.Text;

namespace AttrectoTest.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }

    public static IServiceCollection AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuthUserService, AuthUserService>();
        services.AddScoped<IAppUserService, AppUserService>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UserIdBehavior<,>));

        services.AddScoped<FeedMapper>();

        var jwt = configuration.GetSection("Jwt");
        services.Configure<JwtSettings>(jwt);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt["Issuer"],
                    ValidAudience = jwt["Audience"],
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}
