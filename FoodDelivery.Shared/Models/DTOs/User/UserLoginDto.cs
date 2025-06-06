using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Shared.Models.DTOs.User;

public class UserLoginDto
{
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
}