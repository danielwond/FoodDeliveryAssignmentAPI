using FoodDelivery.Services.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers;

[Route("api/sample")]
[ApiController]
[Authorize("AdminPolicy")]
public class SampleController() : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Authorized");
    }
}