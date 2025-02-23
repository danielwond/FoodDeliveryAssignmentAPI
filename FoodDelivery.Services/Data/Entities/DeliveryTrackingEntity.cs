namespace FoodDelivery.Services.Data.Entities;

public class DeliveryTrackingEntity : BaseEntity
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public OrderEntity Order { get; set; }
}