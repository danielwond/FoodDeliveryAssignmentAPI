using System.Net;
using FoodDelivery.Services.Services.OrderService;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Models.DTOs.Order;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers;

[ApiController]
[Route("api/orders")]

public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var result = await orderService.GetAllOrders();
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto id)
    {
        var result = await orderService.CreateOrder(id);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpPatch("{orderId:guid}/assigntodeliverydriver/{deliveryId:guid}")]
    public async Task<IActionResult> AssignToDeliveryDriver(Guid orderId, Guid deliveryId)
    {
        var result = await orderService.AssignDeliveryToOrder(orderId, deliveryId);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpPatch("{orderId:guid}/updateorderstatus/{id:int}")]
    public async Task<IActionResult> UpdateOrderStatus(Guid orderId, int id)
    {
        var result = await orderService.ChangeOrderStatus(orderId, (OrderStatusEnumId)id);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }
    
    [HttpPatch("{orderId:guid}/cancelorder")]
    public async Task<IActionResult> CancelOrder(Guid orderId)
    {
        var result = await orderService.CancelOrder(orderId);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }
    
    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetOrder(Guid customerId)
    {
        var result = await orderService.GetAllOrdersWithUserId(customerId);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    [HttpPatch("{orderId:guid}/coordinates/{longitude}/{latitude}")]
    public async Task<IActionResult> UpdateDriverCoordinates(Guid orderId, string longitude, string latitude)
    {
        var result = await orderService.UpdateDriverCoordinates(orderId, longitude, latitude);
        return result.isSuccess ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, result);
    }
    
}