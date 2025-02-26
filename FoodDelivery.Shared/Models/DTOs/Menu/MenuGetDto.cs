namespace FoodDelivery.Shared.Models.DTOs.Menu;

public class MenuGetDto
{
    public Guid ID { get; set; }
    public string FoodName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<string> ImagesOfTheFood { get; set; }
}