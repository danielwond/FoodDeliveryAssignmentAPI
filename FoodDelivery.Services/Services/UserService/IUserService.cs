using FoodDelivery.Shared;
using FoodDelivery.Shared.Models.DTOs.User;
using Microsoft.AspNetCore.Http;

namespace FoodDelivery.Services.Services.UserService;

public interface IUserService
{
    public Task<ServiceResponse<string>> RegisterUser(UserRegisterDto user);
}