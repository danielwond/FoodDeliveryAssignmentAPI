using System.Security.Claims;
using FoodDelivery.Services.Data;
using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Shared;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Helpers;
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
            Data = "",
            Message = "User created successfully.",
            isSuccess = true
        };
    }

    public async Task<ServiceResponse<string>> Login(UserLoginDto user)
    {
        var loggedInUser = await context.Users.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
        if (loggedInUser == null ||
            !PasswordHelpers.VerifyPasswordHash(user.Password, loggedInUser.PasswordHash, loggedInUser.PasswordSalt))
        {
            return new ServiceResponse<string>()
            {
                Data = "",
                Message = "Invalid credentials."
            };
        }
        else
        {
            var role = loggedInUser.UserRole;
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Sid, loggedInUser.ID.ToString()),
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
    }
}