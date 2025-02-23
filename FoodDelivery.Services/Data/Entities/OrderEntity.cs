using FoodDelivery.Shared.Enums;

namespace FoodDelivery.Services.Data.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? DeliveryPersonId { get; set; }
    public OrderStatusEnumId Status { get; set; }
    public decimal TotalPrice { get; set; }

    public UserEntity Customer { get; set; }
    public UserEntity? DeliveryPerson { get; set; }
    public ICollection<OrderItemEntity> OrderItems { get; set; }
}