namespace FoodDelivery.Services.Data.Entities;

public class OrderItemEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid MenuItemId { get; set; }
    public int Quantity { get; set; }
    public MenuItemEntity MenuItem { get; set; }
}
