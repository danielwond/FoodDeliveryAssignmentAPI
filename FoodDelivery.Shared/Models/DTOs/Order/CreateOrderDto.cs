namespace FoodDelivery.Shared.Models.DTOs.Order;

public class CreateOrderDto
{
    public Guid CustomerId { get; set; }
    public string DeliveryLocationLongitude { get; set; }
    public string DeliveryLocationLatitude { get; set; }
    public ICollection<CreateOrderItemDto> OrderItems { get; set; }
}