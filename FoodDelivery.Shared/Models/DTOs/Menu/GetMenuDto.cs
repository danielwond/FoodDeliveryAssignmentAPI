using System.Text.Json.Serialization;

namespace FoodDelivery.Shared.Models.DTOs.Menu;

public class GetMenuDto
{
    public Guid ID { get; set; }
    [JsonPropertyName("name")]
    public string FoodName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<string> ImagesOfTheFood { get; set; }
}