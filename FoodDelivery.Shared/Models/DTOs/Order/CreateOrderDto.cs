using FoodDelivery.Shared.Enums;

namespace FoodDelivery.Shared.Models.DTOs.Order;

//required this.subtotal,
//required this.tax,
//required this.total,

public class CreateOrderDto
{
    public Guid CustomerId { get; set; }
    public string? Message { get; set; }
    public double subtotal { get; set; }
    public double tax { get; set; }
    public double total { get; set; }
    public PaymentMethodsEnum PaymentMethod { get; set; }
    public ICollection<CreateOrderItemDto> OrderItems { get; set; }
}