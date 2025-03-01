namespace FoodDelivery.Services.Data.Entities;

public class DeliveryTrackingEntity : BaseEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public OrderEntity Order { get; set; }
    public string Longitude { get; set; }
    public string Latitude { get; set; }
}