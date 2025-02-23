using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FoodDelivery.DataAccess;
using FoodDelivery.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.Configurations;

public static class DependencyInjectionConfigurations
{
    public static void ConfigureDbInjections(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("default");
        services.AddDbContext<DataContext>(x => x.UseMySql(ServerVersion.AutoDetect(connectionString)));
    }
}