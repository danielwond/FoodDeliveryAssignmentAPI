using FoodDelivery.Services.Data;
using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Shared;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Helpers;
using FoodDelivery.Shared.Models.DTOs.Menu;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.Services.MenuService;

public class MenuService(DataContext context, IHttpContextAccessor httpContextAccessor) : IMenuService
{
    public async Task<ServiceResponse<IEnumerable<GetMenuDto>>> GetMenus()
    {
        var result = await context.Menus.AsNoTracking().ToListAsync();
        var data = result.Select(x => new GetMenuDto()
        {
            ID = x.ID,
            ImagesOfTheFood = x.ImagesOfTheFood.Split(',').Select(addServerURL).ToList(),
            Price = x.Price,
            Description = x.Description,
            FoodName = x.FoodName,
        }).ToList();
        return new ServiceResponse<IEnumerable<GetMenuDto>>()
        {
            Data = data,
            Message = "All Fetched",
            isSuccess = true
        };
    }

    public async Task<ServiceResponse<MenuItemEntity>> GetMenu(Guid id)
    {
        var result = await context.Menus.AsNoTracking().Where(x => x.ID == id).FirstOrDefaultAsync();
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
            var result = await context.Menus.Where(x => x.FoodName == dto.FoodName).FirstOrDefaultAsync();
            if (result != null)
            {
                return new ServiceResponse<MenuItemEntity>()
                {
                    Message = "Menu already exists",
                };
            }
            
            var imagePaths = new List<string>();
            foreach (var img in dto.Images)
            {
                var path = await FileHelpers.UploadImage(img, FileTypeEnum.ImgMenu);
                imagePaths.Add(path);
            }
            var imgs = string.Join(",", imagePaths);
            
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
            await context.Menus.AddAsync(newMenu);
            await context.SaveChangesAsync();

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
            var result = await context.Menus.Where(x => x.ID == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                return new ServiceResponse<MenuItemEntity>()
                {
                    Message = "Menu Not Found",
                };
            }
            
            result.Status = !result.Status;
            
            context.Menus.Update(result);
            await context.SaveChangesAsync();

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
            var result = await context.Menus.Where(x => x.ID == Id).FirstOrDefaultAsync();
            if (result == null)
            {
                return new ServiceResponse<Guid>()
                {
                    Message = "Menu Not Found",
                };
            }
            context.Menus.Remove(result);
            await context.SaveChangesAsync();
            
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

    private string addServerURL(string path)
    {
        var request = httpContextAccessor.HttpContext?.Request;

        if (request == null)
            return path;
        var myURL = "/api/files/get?filePath=";
        var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}{myURL}{path}";
        return baseUrl;    
    }
}