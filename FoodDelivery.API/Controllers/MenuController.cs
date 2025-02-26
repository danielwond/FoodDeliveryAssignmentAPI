using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Services.Services.MenuService;
using FoodDelivery.Shared;
using FoodDelivery.Shared.Models.DTOs.Menu;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers;

[ApiController]
[Route("api/menu")]

public class MenuController(IMenuService menuService) : ControllerBase
{
    private readonly IMenuService _menuService = menuService;

    [HttpGet]
    public async Task<IActionResult> GetAllMenus()
    {
        var result = await _menuService.GetMenus();
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpPost]
    public async Task<IActionResult> AddMenu([FromForm] MenuAddDto addDto)
    {
        var result = await _menuService.AddMenu(addDto);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpPut("{id:guid}/update/status")]
    public async Task<IActionResult> ChangeMenuStatus(Guid id)
    {
        var result = await _menuService.ChangeMenuStatus(id);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMenu(Guid Id)
    {
        var result = await _menuService.DeleteMenu(Id);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }
}