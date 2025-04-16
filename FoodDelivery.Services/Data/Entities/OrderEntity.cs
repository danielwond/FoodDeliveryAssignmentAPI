using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FoodDelivery.Shared.Enums;

namespace FoodDelivery.Services.Data.Entities;

public class OrderEntity : BaseEntity
{
    public int Id { get; set; }
    public Guid CustomerId { get; set; }
    public OrderStatusEnumId Status { get; set; }
    public decimal total { get; set; }
    public UserEntity Customer { get; set; }
    public string? Message { get; set; }
    public double subtotal  { get; set; }
    public double tax { get; set; }
    
    public PaymentMethodsEnum PaymentMethod { get; set; }
    public ICollection<OrderItemEntity> OrderItems { get; set; }
}
