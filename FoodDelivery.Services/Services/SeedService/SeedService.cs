using FoodDelivery.Services.Data;
using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.Services.SeedService;

public class SeedService(DataContext context) : ISeedService
{
    private readonly DataContext _context = context;
    
    //lat long
    //43.03825° N, 76.13116° W

    public async Task<string> SeedAsync()
    {
        try
        {
            //seed account
            var accounts = await SeedAccounts();
            if (accounts == false)
            {
                throw new Exception("Seeding Accounts failed");
            }
            var configurations = await SeedConfigurations();
            if (configurations == false)
            {
                throw new Exception("Seeding Configurations failed");
            }

            await SeedMenus();
            return "";
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
            var menus = await _context.Menus.ToListAsync();
            if (menus.Count == 0)
            {
                var newMenu = new MenuItemEntity()
                {
                    ID = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow.ToUniversalTime(),
                    ModifiedOn = DateTime.UtcNow.ToUniversalTime(),
                    Description = "Food1",
                    Status = true,
                    ImagesOfTheFood = "",
                    Price = 50,
                    FoodName = "Food1",
                };
                await _context.Menus.AddAsync(newMenu);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    //Seed Accounts
    private async Task<bool> SeedAccounts()
    {
        var result = true;
        
        var users = await _context.Users.ToListAsync();
        
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
            await _context.Users.AddAsync(admin);
            var dbResult = await _context.SaveChangesAsync();
            
            result = dbResult != 0;        
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
            await _context.Users.AddAsync(driver);
            var dbResult = await _context.SaveChangesAsync();
            
            result = dbResult != 0 ? true :  false;
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
            await _context.Users.AddAsync(customer);
            var dbResult = await _context.SaveChangesAsync();
            
            result = dbResult != 0;
        }
        return result;
    }
    //Seed Configurations
    private async Task<bool> SeedConfigurations()
    {
        var result = false;
        var configs = await _context.Configurations.ToListAsync();

        if (configs.Count != 0) return result;
        
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
        await _context.Configurations.AddRangeAsync(longitude, latitude);
        var dbResult= await _context.SaveChangesAsync();
            
        result = dbResult != 0;
        return result;
        
    }
}