namespace FoodDelivery.DataAccess.Entities;

public class OrderItemEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid MenuItemId { get; set; }
    public int Quantity { get; set; }

    public OrderEntity Order { get; set; }
    public MenuItemEntity MenuItem { get; set; }
}
