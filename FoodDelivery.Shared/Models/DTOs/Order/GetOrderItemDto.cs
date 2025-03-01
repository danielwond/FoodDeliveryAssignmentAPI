using FoodDelivery.Shared.Models.DTOs.Menu;

namespace FoodDelivery.Shared.Models.DTOs.Order;

public class GetOrderItemDto
{
    public int Quantity { get; set; }

    public GetMenuDto MenuItem { get; set; }
}