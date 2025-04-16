using FoodDelivery.Services.Data;
using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Services.Services.MenuService;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Helpers;
using FoodDelivery.Shared.Models.DTOs.Menu;
using FoodDelivery.Shared.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FoodDelivery.Services.Services.SeedService;

public class SeedService(DataContext context, IMenuService menuService, IOptions<SeedDataOptions> seedOptions) : ISeedService
{
    private readonly SeedDataOptions _seedOptions = seedOptions.Value;
    //lat long
    //43.03825° N, 76.13116° W

    public async Task SeedAsync()
    {
        try
        {
            //seed account
            if (_seedOptions.SeedAccounts)
            {
                await SeedAccounts();
            }
            
            //seed configurations
            if (_seedOptions.SeedConfigurations)
            {
                await SeedConfigurations();
            }
            
            //seed menus
            if (_seedOptions.SeedMenus)
            {
                await SeedMenus();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task SeedMenus()
    {
        try
        {
            var menus = await context.Menus.ToListAsync();
            if (menus.Count == 0)
            {
                var cheeseBurger = new CreateMenuDto(){
                    FoodName = "Cheese Burger",
                    Description = "A perfectly grilled beef patty topped with a slice of melted American cheese, fresh lettuce, vine-ripened tomatoes, crunchy onions, and dill pickles—all stacked on a warm, toasted sesame seed bun. A timeless favorite with every bite bringing that rich, cheesy goodness.",
                    Price = (decimal)12.5,
                    Images = 
                    [
                        GetFileFromPath("/Users/danny/Desktop/Images/Cheese Burger/cheese-burger-1.jpg"), 
                        GetFileFromPath("/Users/danny/Desktop/Images/Cheese Burger/cheese-burger-2.jpg"), 
                        GetFileFromPath("/Users/danny/Desktop/Images/Cheese Burger/cheese-burger-3.jpg")
                    ]
                };
                var hamburger = new CreateMenuDto(){
                    FoodName = "Classic Hamburger",
                    Description = "A juicy, seasoned beef patty grilled to perfection, topped with crisp lettuce, fresh tomato slices, crunchy onions, and tangy pickles—all nestled in a soft, toasted sesame seed bun. Pure, no-frills flavor that never goes out of style.",
                    Price = 13,
                    Images = 
                    [
                        GetFileFromPath("/Users/danny/Desktop/Images/Hamburger/hamburger-1.jpg"), 
                        GetFileFromPath("/Users/danny/Desktop/Images/Hamburger/hamburger-2.jpg"), 
                        GetFileFromPath("/Users/danny/Desktop/Images/Hamburger/hamburger-3.jpg")
                    ]
                };
                var lasagna = new CreateMenuDto(){
                    FoodName = "Lasagna",
                    Description = "Layers of tender pasta sheets, rich and savory meat sauce, creamy béchamel, and melted mozzarella cheese—baked to golden, bubbly perfection. Every bite delivers a comforting blend of flavors, from hearty ground beef and tomato to smooth, cheesy goodness. A timeless Italian favorite that’s warm, filling, and irresistibly satisfying.",
                    Price = (decimal)15.5,
                    Images = 
                    [
                        GetFileFromPath("/Users/danny/Desktop/Images/Lasagna/lasagna-1.jpg"), 
                        GetFileFromPath("/Users/danny/Desktop/Images/Lasagna/lasagna-2.jpg"), 
                        GetFileFromPath("/Users/danny/Desktop/Images/Lasagna/lasagna-3.jpg")
                    ]
                };
                await menuService.AddMenu(hamburger);
                await menuService.AddMenu(cheeseBurger);
                await menuService.AddMenu(lasagna);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    //Seed Accounts
    private async Task SeedAccounts()
    {
        
        var users = await context.Users.ToListAsync();
        
        var admins = users.Where(x =>x.UserRole == UserRoleEnum.Admin).ToList();
        var customers = users.Where(x =>x.UserRole == UserRoleEnum.Customer).ToList();
        var drivers = users.Where(x =>x.UserRole == UserRoleEnum.Driver).ToList();

        if (admins.Count == 0)
        {
            PasswordHelpers.CreatePasswordHash("admin1", out byte[] passwordHash, out byte[] passwordSalt);
        
            var admin = new UserEntity()
            {
                ID = Guid.NewGuid(),
                Email = "admin1",
                CreatedOn = DateTime.Now.ToUniversalTime(),
                ModifiedOn = DateTime.Now.ToUniversalTime(),
                Status = true,
                FullName = "admin1",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = "admin1",
                UserRole = UserRoleEnum.Admin,
            };
            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }

        if (drivers.Count == 0)
        {
            PasswordHelpers.CreatePasswordHash("driver1", out byte[] passwordHash, out byte[] passwordSalt);

            var driver = new UserEntity()
            {
                ID = Guid.NewGuid(),
                Email = "driver1",
                CreatedOn = DateTime.Now.ToUniversalTime(),
                ModifiedOn = DateTime.Now.ToUniversalTime(),
                Status = true,
                FullName = "driver1",
                PhoneNumber = "driver1",
                UserRole = UserRoleEnum.Driver,
                ProfilePicturePath = "",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
            await context.Users.AddAsync(driver);
            await context.SaveChangesAsync();
        }

        if (customers.Count == 0)
        {
            PasswordHelpers.CreatePasswordHash("customer1", out byte[] passwordHash, out byte[] passwordSalt);

            var customer = new UserEntity()
            {
                ID = Guid.NewGuid(),
                Email = "customer1",
                CreatedOn = DateTime.Now.ToUniversalTime(),
                ModifiedOn = DateTime.Now.ToUniversalTime(),
                Status = true,
                FullName = "customer1",
                PhoneNumber = "customer1",
                UserRole = UserRoleEnum.Customer,
                ProfilePicturePath = "",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
            await context.Users.AddAsync(customer);
            await context.SaveChangesAsync();
        }
    }
    //Seed Configurations
    private async Task SeedConfigurations()
    {
        var configs = await context.Configurations.ToListAsync();
        if (configs.Count == 0)
        {
            var longitude = new ConfigurationsEntity()
            {
                ID = Guid.NewGuid(),
                Name = "Restaurant Longitude",
                Description = "Restaurant Longitude",
                ConfigurationsEnum = ConfigurationsEnum.RestaurantLocationLongitude,
                Value = "43.03825"
            };

            var latitude = new ConfigurationsEntity()
            {
                ID = Guid.NewGuid(),
                Name = "Restaurant Latitude",
                Description = "Restaurant Latitude",
                ConfigurationsEnum = ConfigurationsEnum.RestaurantLocationLatitude,
                Value = "76.13116",
            };
            var name = new ConfigurationsEntity()
            {
                ID = Guid.NewGuid(),
                Name = "Restaurant Name",
                Description = "It is a good restaurant with good food and good people",
                ConfigurationsEnum = ConfigurationsEnum.RestaurantName,
                Value = "QuickBite"
            };
            await context.Configurations.AddRangeAsync(longitude, latitude, name);
            await context.SaveChangesAsync();
        }
    }

    private FormFile GetFileFromPath(string filePath)
    {
        var stream = File.OpenRead(filePath);
        var file = new FormFile(stream, 0, stream.Length, Path.GetFileName(filePath), Path.GetFileName(stream.Name))
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/pdf"
        };
        Console.WriteLine(File.Exists(filePath));
        return file;
    }
}