using AttrectoTest.Notification.Infrastructure.Contracts;
using AttrectoTest.Notification.Infrastructure.Models;
using AttrectoTest.Notification.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AttrectoTest.Notification.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfigurationManager configuration )
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
        if (emailSettings == null ) {
            throw new InvalidOperationException("Missing email settings");
        }

        var emailService = new EmailService(
            credentialsPath: emailSettings.CredentialPath,
            fromEmail: emailSettings.FromEmail,
            tokenPath: emailSettings.TokenPath
        );
        services.AddSingleton<IEmailService>( emailService );
        return services;
    }
}
