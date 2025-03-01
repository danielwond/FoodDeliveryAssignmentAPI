using FoodDelivery.Services.Data;
using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Shared;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Helpers;
using FoodDelivery.Shared.Models.DTOs.Menu;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.Services.MenuService;

public class MenuService(DataContext context) : IMenuService
{
    private readonly DataContext _context = context;

    public async Task<ServiceResponse<IEnumerable<MenuItemEntity>>> GetMenus()
    {
        var result = await _context.Menus.AsNoTracking().ToListAsync();
        return new ServiceResponse<IEnumerable<MenuItemEntity>>()
        {
            Data = result,
            Message = "All Fetched",
            isSuccess = true
        };
    }

    public async Task<ServiceResponse<MenuItemEntity>> GetMenu(Guid id)
    {
        var result = await _context.Menus.AsNoTracking().Where(x => x.ID == id).FirstOrDefaultAsync();
        if (result == null)
        {
            return new ServiceResponse<MenuItemEntity>()
            {
                Message = "No Data Found",
            };
        }
        return new ServiceResponse<MenuItemEntity>()
        {
            Data = result,
            Message = "All Fetched",
            isSuccess = true
        };
    }

    public async Task<ServiceResponse<MenuItemEntity>> AddMenu(CreateMenuDto dto)
    {
        try
        {
            var result = await _context.Menus.Where(x => x.FoodName == dto.FoodName).FirstOrDefaultAsync();
            if (result != null)
            {
                return new ServiceResponse<MenuItemEntity>()
                {
                    Message = "Menu already exists",
                };
            }
            var imgs = string.Empty;
            if (dto.Images.Count != 0)
            {
                foreach (var img in dto.Images)
                {
                    var path = await FileHelpers.UploadImage(img, FileTypeEnum.ImgMenu);
                    imgs += path + ",";
                }
            }
            var newMenu = new MenuItemEntity()
            {
                FoodName = dto.FoodName,
                Description = dto.Description,
                Price = dto.Price,
                ID = Guid.NewGuid(),
                CreatedOn = DateTime.Now.ToUniversalTime(),
                ModifiedOn = DateTime.Now.ToUniversalTime(),
                Status = true,
                ImagesOfTheFood = imgs
            };
            await _context.Menus.AddAsync(newMenu);
            await _context.SaveChangesAsync();

            return new ServiceResponse<MenuItemEntity>()
            {
                Data = newMenu,
                Message = "Menu Added",
                isSuccess = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<MenuItemEntity>> ChangeMenuStatus(Guid Id)
    {
        try
        {
            var result = await _context.Menus.Where(x => x.ID == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                return new ServiceResponse<MenuItemEntity>()
                {
                    Message = "Menu Not Found",
                };
            }
            
            result.Status = !result.Status;
            
            _context.Menus.Update(result);
            await _context.SaveChangesAsync();

            return new ServiceResponse<MenuItemEntity>()
            {
                Data = result,
                Message = "Menu Updated",
                isSuccess = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<Guid>> DeleteMenu(Guid Id)
    {
        try
        {
            var result = await _context.Menus.Where(x => x.ID == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                return new ServiceResponse<Guid>()
                {
                    Message = "Menu Not Found",
                };
            }
            _context.Menus.Remove(result);
            await _context.SaveChangesAsync();
            
            //Delete Files
            var images = result.ImagesOfTheFood.Split(',');
            foreach (var img in images)
            {
                if (Path.Exists(img))
                {
                    FileHelpers.DeleteImage(img);
                }
            }
            return new ServiceResponse<Guid>()
            {
                Data = result.ID,
                Message = "Menu Deleted",
                isSuccess = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}