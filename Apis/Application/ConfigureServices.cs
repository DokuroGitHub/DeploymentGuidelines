using System.Reflection;
using Application.Interfaces;
using Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //TODO: add pipeline behavior
        services.AddFluentValidationAutoValidation(); // use auto = no async validator
        services.AddFluentValidationClientsideAdapters();

        // add services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IChemicalService, ChemicalService>();
        services.AddScoped<IUserService, UserService>();
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
