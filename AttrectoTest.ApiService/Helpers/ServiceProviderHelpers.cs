using AttrectoTest.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;

namespace AttrectoTest.ApiService.Helpers;

internal static class ServiceProviderHelpers
{
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
