using FoodDelivery.Services.Services.UserService;
using FoodDelivery.Shared.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers;

[Route("api/user-account")]
[ApiController]
public class UserAccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    
    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromForm] UserRegisterDto user)
    {
        var result = await _userService.RegisterUser(user);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

}