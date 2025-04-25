using System.Security.Claims;
using FoodDelivery.Services.Data;
using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Shared;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Helpers;
using FoodDelivery.Shared.Models.DTOs.Order;
using FoodDelivery.Shared.Models.DTOs.User;
using FoodDelivery.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FoodDelivery.Services.Services.UserService;

public class UserService(DataContext context, IOptions<JWTOptions> options) : IUserService
{
    private readonly JWTOptions _options = options.Value;

    public async Task<ServiceResponse<string>> RegisterUser(UserRegisterDto user)
    {
        try
        {
            var oldUser = await context.Users.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
            if (oldUser == null)
            {
                PasswordHelpers.CreatePasswordHash(user.Password, out var passwordHash, out var passwordSalt);
                var newUser = new UserEntity()
                {
                    ID = Guid.NewGuid(),
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true,
                    CreatedOn = DateTime.Now.ToUniversalTime(),
                    UserRole = user.UserRole,
                    ModifiedOn = DateTime.Now.ToUniversalTime(),
                    ProfilePicturePath = user.Photo == null ? "" : await FileHelpers.UploadImage(user.Photo, FileTypeEnum.ImgProfile),
                };
        
                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();

                return new ServiceResponse<string>()
                {
                    Message = "User created successfully.",
                    isSuccess = true
                };
            }
            else
            {
                return new ServiceResponse<string>()
                {
                    Message = "User already exists.",
                    isSuccess = false
                };
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> Login(UserLoginDto user)
    {
        var loggedInUser = await context.Users.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
        if (loggedInUser == null ||
            !PasswordHelpers.VerifyPasswordHash(user.Password, loggedInUser.PasswordHash, loggedInUser.PasswordSalt))
        {
            return new ServiceResponse<string>()
            {
                Message = "Invalid credentials."
            };
        }

        var role = loggedInUser.UserRole;
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, loggedInUser.ID.ToString()),
            new(ClaimTypes.Name, loggedInUser.FullName),
            new(ClaimTypes.Role, role.ToString()),
        };
        var token = TokenHelpers.GenerateToken(claims, _options);
        return new ServiceResponse<string>()
        {
            Data = token,
            Message = "User Authenticated.",
            isSuccess = true
        };
    }

    public async Task<List<UserEntity>> GetAllUsers()
    {
        try
        {
            var result = await context.Users.ToListAsync();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> UpdateDriverCoordinates(Guid driverId, string longitude, string latitude)
    {
        try
        {
            var driver = await context.Drivers.Where(x => x.ID == driverId).FirstOrDefaultAsync();
            if (driver == null)
            {
                return new ServiceResponse<string>()
                {
                    Message = "Driver does not exist."
                };
            }

            driver.Longitude = longitude;
            driver.Latitude = latitude;

            context.Update(driver);
            await context.SaveChangesAsync();

            return new ServiceResponse<string>()
            {
                Message = "Driver updated successfully.",
                isSuccess = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<GetDriverLocationDto>> GetDriverLocation(Guid driverId)
    {
        try
        {
            var driver = await context.Drivers.Where(x => x.ID == driverId).FirstOrDefaultAsync();
            if (driver == null)
            {
                return new ServiceResponse<GetDriverLocationDto>()
                {
                    Message = "Driver does not exist."
                };
            }

            return new ServiceResponse<GetDriverLocationDto>()
            {
                Data = new GetDriverLocationDto()
                {
                    Latitude = driver.Latitude,
                    Longitude = driver.Longitude,
                },
                Message = "Driver location updated successfully.",
                isSuccess = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> ChangeDriverStatus(Guid driverId)
    {
        try
        {
            var driver = await context.Drivers.Where(x => x.ID == driverId).FirstOrDefaultAsync();
            if (driver == null)
            {
                return new ServiceResponse<string>()
                {
                    Message = "Driver does not exist."
                };
            }
            driver.DriverActive = !driver.DriverActive;
            context.Update(driver);
            await context.SaveChangesAsync();

            return new ServiceResponse<string>()
            {
                Message = driver.DriverActive ? "Activated" : "Deactivated",
                isSuccess = true,
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> UpdateUserInformation(GetUserDto user)
    {
        try
        {
            var result = await context.Users.Where(x => x.Email == user.email).FirstOrDefaultAsync();
            if (result == null)
            {
                return new ServiceResponse<string>()
                {
                    Message = "Email does not exist.",
                    isSuccess = false
                };
            }
            result.FullName = user.name;
            result.PhoneNumber = user.phone;
            result.Email = user.email;
            
            context.Update(result);
            await context.SaveChangesAsync();

            return new ServiceResponse<string>()
            {
                Message = "User updated successfully.",
                isSuccess = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<GetUserDto>> GetUserInfo(Guid userId)
    {
        try
        {
            var user = await context.Users.Where(x => x.ID == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return new ServiceResponse<GetUserDto>()
                {
                    Message = "User does not exist.",
                    isSuccess = false
                };
            }

            return new ServiceResponse<GetUserDto>()
            {
                Data = new GetUserDto()
                {
                    name = user.FullName,
                    email = user.Email,
                    phone = user.PhoneNumber,
                    id = user.ID.ToString(),
                },
                Message = "User retrieved successfully.",
                isSuccess = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<List<DriverEntity>>> GetAllActiveDrivers()
    {
        try
        {
            var result = await context.Drivers.Where(x => !x.DriverActive).ToListAsync();
            return new ServiceResponse<List<DriverEntity>>()
            {
                Data = result,
                Message = "Fetched drivers.",
                isSuccess = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}