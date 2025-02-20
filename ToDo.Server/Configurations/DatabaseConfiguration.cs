using Microsoft.EntityFrameworkCore;
using ToDo.Server.Data;

namespace ToDo.Server.Configurations;

public static class DatabaseConfiguration
{
    public static void AddCustomDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ToDoDb");

        services.AddDbContext<ToDoDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                var assembly = typeof(ToDoDbContext).Assembly;
                var assemblyName = assembly.GetName();

                sqlServerOptions.MigrationsAssembly(assemblyName.Name);
                sqlServerOptions.EnableRetryOnFailure(
                                 maxRetryCount: 2,
                                 maxRetryDelay: TimeSpan.FromSeconds(30),
                                 errorNumbersToAdd: null);
            });
        });
    }
}
