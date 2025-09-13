using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Persistence.DatabaseContext;
using AttrectoTest.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AttrectoTest.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TestDbContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(11, 3, 0)),
                b => b.MigrationsAssembly(typeof(TestDbContext).Assembly.FullName)
        ));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IFeedRepository, FeedRepository>();
        return services;
    }

    public static void RunDatabaseMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<TestDbContext>();

        var retries = 10;
        var delay = TimeSpan.FromSeconds(5);

        /// Try to apply migrations with retries, because the database might not be available yet.
        while (retries > 0)
        {
            try
            {
                db.Database.Migrate();
                Console.WriteLine("Migration is successful.");
                var userService = scope.ServiceProvider.GetRequiredService<IAppUserService>();
                userService.AddNewUser("test", "Passw0rd!", "User").Wait();
                break;
            }
            catch (Exception ex)
            {
                retries--;
                Console.WriteLine($"Migration is failed, retry after {delay.TotalSeconds}s... ({ex.Message})");
                if (retries == 0) throw;
                Thread.Sleep(delay);
            }
        }
    }

}
