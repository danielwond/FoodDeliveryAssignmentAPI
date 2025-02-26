using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Shared;
using FoodDelivery.Shared.Models.DTOs.Menu;
using FoodDelivery.Shared.Models.DTOs.User;

namespace FoodDelivery.Services.Services.MenuService;

public interface IMenuService
{
    public Task<ServiceResponse<IEnumerable<MenuItemEntity>>> GetMenus();
    public Task<ServiceResponse<MenuItemEntity>> GetMenu(Guid id);
    public Task<ServiceResponse<MenuItemEntity>> AddMenu(MenuAddDto addDto);

    public Task<ServiceResponse<MenuItemEntity>> ChangeMenuStatus(Guid Id);
    public Task<ServiceResponse<Guid>> DeleteMenu(Guid Id);
}