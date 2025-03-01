using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Models.DTOs.User;

namespace FoodDelivery.Shared.Models.DTOs.Order;

public class GetOrderCustomerDto
{
    public Guid Id { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }

    public GetDriverDto? DeliveryPerson { get; set; }
    public ICollection<GetOrderItemDto> OrderItems { get; set; }
}