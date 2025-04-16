using System.Text.Json.Serialization;

namespace FoodDelivery.Services.Data.Entities;

public class MenuItemEntity : BaseEntity
{
    public Guid ID { get; set; }
    [JsonPropertyName("Name")]
    public string FoodName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    [JsonPropertyName("imageUrl")]
    public string ImagesOfTheFood { get; set; }
}