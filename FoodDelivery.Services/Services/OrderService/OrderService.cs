using FoodDelivery.Services.Data;
using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Shared;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Models.DTOs.Menu;
using FoodDelivery.Shared.Models.DTOs.Order;
using FoodDelivery.Shared.Models.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.Services.OrderService;

public class OrderService(DataContext context, IHttpContextAccessor httpContextAccessor) : IOrderService
{
    public async Task<ServiceResponse<List<OrderEntity>>> GetAllOrders()
    {
        try
        {
            var result = await context.Orders.Include(x => x.OrderItems).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result.Count == 0
                ? new ServiceResponse<List<OrderEntity>>() { Message = "No orders found" }
                : new ServiceResponse<List<OrderEntity>>()
                    { Data = result, Message = "Orders found", isSuccess = true };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

public async Task<ServiceResponse<OrderEntity>> CreateOrder(CreateOrderDto data)
{
    try
    {
        var customer = await context.Users.FirstOrDefaultAsync(x => x.ID == data.CustomerId);
        if (customer == null)
        {
            return new ServiceResponse<OrderEntity>() { Message = "Customer not found" };
        }

        var menuItemIds = data.OrderItems.Select(x => x.MenuItemId).ToList();
        var menuItems = await context.Menus
            .Where(x => menuItemIds.Contains(x.ID))
            .ToDictionaryAsync(x => x.ID, x => x);

        if (menuItems.Count != menuItemIds.Count)
        {
            return new ServiceResponse<OrderEntity>() { Message = "One or more menu items not found" };
        }

        var orderItems = new List<OrderItemEntity>();
        decimal totalPrice = 0;

        foreach (var item in data.OrderItems)
        {
            var menuItem = menuItems[item.MenuItemId];
            totalPrice += menuItem.Price * item.Quantity;
            
            orderItems.Add(new OrderItemEntity
            {
                Id = Guid.NewGuid(),
                MenuItemId = menuItem.ID,
                Quantity = item.Quantity,
                MenuItem = menuItem
            });
        }

        var order = new OrderEntity
        {
            CustomerId = data.CustomerId,
            CreatedOn = DateTime.UtcNow,
            Status = OrderStatusEnumId.Pending,
            ModifiedOn = DateTime.UtcNow,
            Message = data.Message,
            PaymentMethod = data.PaymentMethod,
            subtotal = data.subtotal,
            tax = data.tax,
            total = totalPrice,
            OrderItems = orderItems,
            Customer = customer
        };

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return new ServiceResponse<OrderEntity>
        {
            Data = order,
            Message = "Order created successfully",
            isSuccess = true
        };
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return new ServiceResponse<OrderEntity>
        {
            Message = $"Error creating order: {e.Message}"
        };
    }
}

    public async Task<ServiceResponse<OrderEntity>> ChangeOrderStatus(int orderId, OrderStatusEnumId statudId)
    {
        try
        {
            var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null)
            {
                return new ServiceResponse<OrderEntity>() { Message = "Order not found" };
            }
            
            //TODO: if you have time, add push notification to the user.. notifying them of the order status.
            
            order.Status = statudId;
            context.Orders.Update(order);
            
            await context.SaveChangesAsync();

            return new ServiceResponse<OrderEntity>()
                { Data = order, Message = "Order Status Changed", isSuccess = true };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<List<GetOrderCustomerDto>>> GetAllOrdersWithUserId(Guid userId)
    {
        var response = new ServiceResponse<List<GetOrderCustomerDto>>();
        try
        {
            // Fetch data from database first
            var orders = await context.Orders
                .Where(x => x.CustomerId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .AsNoTracking()
                .ToListAsync(); // Execute the query and retrieve the data

            // Now process the data in memory
            var result = orders.Select(x => new GetOrderCustomerDto()
            {
                Id = x.Id,
                Status = x.Status.ToString(),
                TotalPrice = x.total,
                OrderItems = x.OrderItems.Select(oi => new GetOrderItemDto()
                {
                    Quantity = oi.Quantity,
                    MenuItem = new GetMenuDto()
                    {
                        ID = oi.MenuItem.ID,
                        Price = oi.MenuItem.Price,
                        Description = oi.MenuItem.Description,
                        FoodName = oi.MenuItem.FoodName,
                        ImagesOfTheFood = oi.MenuItem.ImagesOfTheFood.Length != 0
                            ? oi.MenuItem.ImagesOfTheFood.Split(',').Select(addServerURL).ToList() 
                            : [],
                    },
                }).ToList(),
                PaymentMethod = x.PaymentMethod,
                orderDate = x.CreatedOn,
                Tax = x.tax,
                SubTotal = x.subtotal,
            }).ToList();

            response.Data = result;
            response.Message = "Orders found";
            response.isSuccess = true;
        }
        catch (Exception e)
        {
            response.Message = $"Error: {e.Message}";
            response.isSuccess = false;
        }
        return response;
    }

    public async Task<ServiceResponse<int>> CancelOrder(int orderId)
    {
        try
        {
            var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null)
            {
                return new ServiceResponse<int>() { Message = "Order not found" };
            }
            
            order.Status = OrderStatusEnumId.Cancelled;
            
            context.Orders.Update(order);
            await context.SaveChangesAsync();
            
            return new ServiceResponse<int>() { Data = order.Id, Message = "Order cancelled", isSuccess = true };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    private string addServerURL(string path)
    {
        var request = httpContextAccessor.HttpContext?.Request;

        if (request == null)
            return path;
        var myURL = "/api/files/get?filePath=";
        var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}{myURL}{path}";
        return baseUrl;    
    }
}