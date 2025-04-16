using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Models.DTOs.User;

namespace FoodDelivery.Shared.Models.DTOs.Order;

public class GetOrderCustomerDto
{
    public int Id { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public double SubTotal { get; set; }
    public double Tax { get; set; }
    public DateTime orderDate { get; set; }
    public PaymentMethodsEnum PaymentMethod { get; set; }
    public ICollection<GetOrderItemDto> OrderItems { get; set; }
}