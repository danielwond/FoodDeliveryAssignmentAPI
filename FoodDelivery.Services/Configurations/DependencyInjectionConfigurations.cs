using System.Text;
using FoodDelivery.Services.Data;
using FoodDelivery.Services.Services.MenuService;
using FoodDelivery.Services.Services.OrderService;
using FoodDelivery.Services.Services.SeedService;
using FoodDelivery.Services.Services.UserService;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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

    public static void ConfigureOptionInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JWTOptions>(configuration.GetSection("JWTOptions"));
        services.Configure<SeedDataOptions>(configuration.GetSection("SeedData"));
    }
    public static void ConfigureServicesInjection(this IServiceCollection services)
    {
        services.AddScoped<ISeedService, SeedService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IOrderService, OrderService>();
        
        services.AddHttpContextAccessor();
    }

    public static void ConfigureAuthInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWTOptions:ValidIssuer"],
                    ValidAudience = configuration["JWTOptions:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTOptions:Secret"]))
                };
            });
        services.AddAuthorizationBuilder()
            .AddPolicy("AdminPolicy", policy => policy.RequireRole(UserRoleEnum.Admin.ToString()))
            .AddPolicy("UserPolicy", policy => policy.RequireRole(UserRoleEnum.Customer.ToString()));
    }
}