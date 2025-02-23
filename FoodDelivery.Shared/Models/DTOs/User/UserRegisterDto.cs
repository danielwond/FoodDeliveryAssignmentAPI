using FoodDelivery.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace FoodDelivery.Shared.Models.DTOs.User;

public class UserRegisterDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public UserRoleEnum UserRole { get; set; }
    public IFormFile? Photo { get; set; }
}