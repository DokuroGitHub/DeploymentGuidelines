using Application.Commons.Models;
using Application.Interfaces;
using Application.Repositories;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        Appsettings appsettings)
    {
        if (appsettings.UseInMemoryDatabase)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDb");
                options.EnableSensitiveDataLogging();
            });
        }
        else
        {
            var connectionString = appsettings.DatabaseConnection;
            Console.WriteLine($"connectionString: {connectionString}");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    connectionString,
                    builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
                options.EnableSensitiveDataLogging();
            });
        }

        // repositories
        services.AddScoped<IChemicalRepository, ChemicalRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
