using AttrectoTest.Aim.Application.Contracts.Identity;
using AttrectoTest.Aim.Application.Contracts.Persistence;
using AttrectoTest.Aim.Persistence.DatabaseContext;
using AttrectoTest.Aim.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AttrectoTest.Aim.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddDbContext<TestDbContext>(options =>
        //    options.UseMySql(
        //        configuration.GetConnectionString("DefaultConnection"),
        //        new MySqlServerVersion(new Version(11, 3, 0)),
        //        b => b.MigrationsAssembly(typeof(TestDbContext).Assembly.FullName)
        //));

        services.AddDbContextFactory<TestDbContext>(options =>
    options.UseMySql(
        configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(11, 3, 0)),
        b => b.MigrationsAssembly(typeof(TestDbContext).Assembly.FullName)
));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
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
            if (db.Database.CanConnect())
            {
                if (db.Database.GetPendingMigrations().Count() == 0)
                {
                    Console.WriteLine("No pending migrations.");
                    break;
                }
                db.Database.Migrate();
                Console.WriteLine("Migration is successful.");
                var userService = scope.ServiceProvider.GetRequiredService<IAppUserService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();
                var seeder = new Seed.DbSeeder(dbContext, userService);
                seeder.SeedAsync().Wait();
                break;
            } else {
                retries--;
                Console.WriteLine($"Database not available yet. Retrying in {delay.TotalSeconds} seconds... ({retries} retries left)");
                Thread.Sleep(delay);
            } 
        }


    }

}
