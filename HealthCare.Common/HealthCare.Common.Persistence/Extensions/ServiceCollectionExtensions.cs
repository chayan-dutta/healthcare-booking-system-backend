using HealthCare.Common.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;


namespace HealthCare.Common.Persistence.Extensions;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// An extension method for Database connectivity
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="configSectionName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IServiceCollection AddPostgresDbContext<TContext>(this IServiceCollection services, IConfiguration configuration, string configSectionName) 
        where TContext : DbContext
    {

        var section = configuration.GetSection(configSectionName);
        var json = JsonSerializer.Serialize(section.GetChildren().ToDictionary(x => x.Key, x => x.Value));
        var dbOptions = JsonSerializer.Deserialize<DbConnectionOptions>(json);

        // Validate options
        if (dbOptions == null ||
            string.IsNullOrEmpty(dbOptions.Host) ||
            string.IsNullOrEmpty(dbOptions.Port) ||
            string.IsNullOrEmpty(dbOptions.Database) ||
            string.IsNullOrEmpty(dbOptions.Username) ||
            string.IsNullOrEmpty(dbOptions.Password))
        {
            throw new ArgumentException($"Invalid or missing database configuration under section '{configSectionName}'");
        }

        int port = int.Parse(dbOptions.Port);

        // Build connection string
        var connectionString = $"Host={dbOptions.Host};Port={dbOptions.Port};Database={dbOptions.Database};Username={dbOptions.Username};Password={dbOptions.Password}";

        // Register DbContext with PostgreSQL provider
        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}
