using FoodDelivery.Services.Data;
using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Shared;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Helpers;
using FoodDelivery.Shared.Models.DTOs.User;
using Microsoft.AspNetCore.Http;

namespace FoodDelivery.Services.Services.UserService;

public class UserService(DataContext context) : IUserService
{
    private readonly DataContext _context = context;

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
        
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return new ServiceResponse<string>()
        {
            Data = "",
            Message = "User created successfully.",
            isSuccess = true
        };
    }
}