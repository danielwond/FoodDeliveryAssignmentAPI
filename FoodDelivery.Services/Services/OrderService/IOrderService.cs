using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Shared;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Models.DTOs.Order;

namespace FoodDelivery.Services.Services.OrderService;

public interface IOrderService
{
    public Task<ServiceResponse<List<OrderEntity>>> GetAllOrders();
    public Task<ServiceResponse<OrderEntity>> CreateOrder(CreateOrderDto id);
    
    //assign order to delivery personnel
    public Task<ServiceResponse<UserEntity>> AssignDeliveryToOrder(Guid orderId, Guid userId);
    
    //change order status
    public Task<ServiceResponse<OrderEntity>> ChangeOrderStatus(Guid orderId, OrderStatusEnumId statudId);

    public Task<ServiceResponse<List<GetOrderCustomerDto>>> GetAllOrdersWithUserId(Guid userId);
    
    //Cancel Order
    public Task<ServiceResponse<Guid>> CancelOrder(Guid orderId);
    
    //Update Delivery Driver Coordinates
    public Task<ServiceResponse<string>> UpdateDriverCoordinates(Guid orderId, string longitude, string latitude);
    
}