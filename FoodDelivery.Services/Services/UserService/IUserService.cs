using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Shared;
using FoodDelivery.Shared.Models.DTOs.Order;
using FoodDelivery.Shared.Models.DTOs.User;
using Microsoft.AspNetCore.Http;

namespace FoodDelivery.Services.Services.UserService;

public interface IUserService
{
    public Task<ServiceResponse<string>> RegisterUser(UserRegisterDto user);
    public Task<ServiceResponse<string>> Login(UserLoginDto user);
    public Task<List<UserEntity>> GetAllUsers();
    
    //Update Delivery Driver Coordinates
    public Task<ServiceResponse<string>> UpdateDriverCoordinates(Guid driverId, string longitude, string latitude);
    
    public Task<ServiceResponse<GetDriverLocationDto>> GetDriverLocation(Guid driverId);
    
    public Task<ServiceResponse<string>> ChangeDriverStatus(Guid driverId);
    
    public Task<ServiceResponse<string>> UpdateUserInformation(GetUserDto user);

    public Task<ServiceResponse<GetUserDto>> GetUserInfo(Guid userId);
}