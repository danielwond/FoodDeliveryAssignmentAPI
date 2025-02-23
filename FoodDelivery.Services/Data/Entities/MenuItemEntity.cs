namespace FoodDelivery.Services.Data.Entities;

public class MenuItemEntity : BaseEntity
{
    public Guid ID { get; set; }
    public string FoodName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImagesOfTheFood { get; set; }
}