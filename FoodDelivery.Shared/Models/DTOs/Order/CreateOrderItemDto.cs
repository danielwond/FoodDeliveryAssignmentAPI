namespace FoodDelivery.Shared.Models.DTOs.Order;

public class CreateOrderItemDto
{
    public Guid MenuItemId { get; set; }
    public int Quantity { get; set; }
}