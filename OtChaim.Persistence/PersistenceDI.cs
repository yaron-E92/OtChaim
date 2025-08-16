using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Users;

namespace OtChaim.Persistence;

public static class PersistenceDI
{
    /// <summary>
    /// Registers the OtChaim persistence layer (DbContext and repositories) with the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="useInMemory">If true, uses InMemory provider; otherwise, expects further configuration.</param>
    /// <param name="dbName">The name of the in-memory database (if used).</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        bool useInMemory = true,
        string dbName = "OtChaimDb")
    {
        if (useInMemory)
        {
            services.AddDbContext<OtChaimDbContext>(options =>
                options.UseInMemoryDatabase(dbName));
        }
        // else: consumer can configure DbContext externally (e.g., UseSqlServer, UseSqlite)

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmergencyRepository, EmergencyRepository>();

        return services;
    }
}
