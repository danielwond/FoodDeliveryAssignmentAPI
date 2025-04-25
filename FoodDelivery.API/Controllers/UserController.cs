using FoodDelivery.Services.Services.UserService;
using FoodDelivery.Shared.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers;

[Route("api/user")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromForm] UserRegisterDto user)
    {
        var result = await userService.RegisterUser(user);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto user)
    {
        var result = await userService.Login(user);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    //To be removed after implementation
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await userService.GetAllUsers();
        return Ok(result);  
    }
    [HttpPatch("drivers/{driverId:guid}/coordinates/{longitude}/{latitude}")]
    public async Task<IActionResult> UpdateDriverCoordinates(Guid driverId, string longitude, string latitude)
    {
        var result = await userService.UpdateDriverCoordinates(driverId, longitude, latitude);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpGet("drivers/{driverId:guid}")]
    public async Task<IActionResult> GetDriverLocation(Guid driverId)
    {
        var result = await userService.GetDriverLocation(driverId);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpPatch("drivers/{driverId:guid}/status")]
    public async Task<IActionResult> UpdateDriverStatus(Guid driverId)
    {
        var result = await userService.ChangeDriverStatus(driverId);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var result = await userService.GetUserInfo(id);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser(GetUserDto user)
    {
        var result = await userService.UpdateUserInformation(user);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }
}