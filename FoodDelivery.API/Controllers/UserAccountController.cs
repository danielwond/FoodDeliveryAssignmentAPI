using FoodDelivery.Services.Services.UserService;
using FoodDelivery.Shared.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers;

[Route("api/user-account")]
[ApiController]
public class UserAccountController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromForm] UserRegisterDto user)
    {
        var result = await userService.RegisterUser(user);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] UserLoginDto user)
    {
        var result = await userService.Login(user);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }
}