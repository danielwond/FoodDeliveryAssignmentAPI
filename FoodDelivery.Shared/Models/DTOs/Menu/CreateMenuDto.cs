using Microsoft.AspNetCore.Http;

namespace FoodDelivery.Shared.Models.DTOs.Menu;

public class CreateMenuDto
{
    public string FoodName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<IFormFile> Images { get; set; }
}