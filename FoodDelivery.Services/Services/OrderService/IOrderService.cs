using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Shared;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Models.DTOs.Order;
using Microsoft.IdentityModel.Tokens;
 
namespace FoodDelivery.Services.Services.OrderService;

public interface IOrderService
{
    public Task<ServiceResponse<List<OrderEntity>>> GetAllOrders();
    public Task<ServiceResponse<OrderEntity>> CreateOrder(CreateOrderDto data);
    //assign order to delivery personnel
    //change order status
    public Task<ServiceResponse<OrderEntity>> ChangeOrderStatus(int orderId, OrderStatusEnumId statudId);
    public Task<ServiceResponse<List<GetOrderCustomerDto>>> GetAllOrdersWithUserId(Guid userId);
    //Cancel Order
    public Task<ServiceResponse<int>> CancelOrder(int orderId);
    
}