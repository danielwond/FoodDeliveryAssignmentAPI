using FoodDelivery.Services.Data;
using FoodDelivery.Services.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodDelivery.Services.Configurations;

public static class DependencyInjectionConfigurations
{
    public static void ConfigureDbInjections(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("default");
        services.AddDbContext<DataContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    }

    public static void ConfigureServicesInjection(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
}