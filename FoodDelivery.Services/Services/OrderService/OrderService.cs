using FoodDelivery.Services.Data;
using FoodDelivery.Services.Data.Entities;
using FoodDelivery.Shared;
using FoodDelivery.Shared.Enums;
using FoodDelivery.Shared.Models.DTOs.Menu;
using FoodDelivery.Shared.Models.DTOs.Order;
using FoodDelivery.Shared.Models.DTOs.User;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.Services.OrderService;

public class OrderService(DataContext context) : IOrderService
{
    public async Task<ServiceResponse<List<OrderEntity>>> GetAllOrders()
    {
        try
        {
            var result = await context.Orders.OrderByDescending(x => x.CreatedOn).ToListAsync();
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

    public async Task<ServiceResponse<OrderEntity>> CreateOrder(CreateOrderDto id)
    {
        try
        {
            var orderId = Guid.NewGuid();
            decimal totalPrice = 0;

            // Validate customer
            var customer = await context.Users.FirstOrDefaultAsync(x => x.ID == id.CustomerId);
            if (customer == null)
            {
                return new ServiceResponse<OrderEntity>() { Message = "Customer not found" };
            }

            // Validate menu items and create order items
            var menuItems = new List<OrderItemEntity>();
            foreach (var orderItem in id.OrderItems)
            {
                var menuItem = await context.Menus.FirstOrDefaultAsync(x => x.ID == orderItem.MenuItemId);
                if (menuItem == null)
                {
                    return new ServiceResponse<OrderEntity>() { Message = "Menu item not found" };
                }

                menuItems.Add(new OrderItemEntity()
                {
                    Id = Guid.NewGuid(),
                    MenuItemId = menuItem.ID,
                    Quantity = orderItem.Quantity,
                    MenuItem = menuItem,
                    OrderId = orderId
                });
                totalPrice += menuItem.Price;
            }

            // Create order
            var order = new OrderEntity()
            {
                Id = orderId,
                CustomerId = id.CustomerId,
                CreatedOn = DateTime.UtcNow,
                OrderItems = menuItems,
                Status = OrderStatusEnumId.Pending,
                Customer = customer,
                TotalPrice = totalPrice,
                ModifiedOn = DateTime.UtcNow,
                DeliveryLocationLatitude = id.DeliveryLocationLatitude,
                DeliveryLocationLongitude = id.DeliveryLocationLongitude
            };

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            return new ServiceResponse<OrderEntity>() { Data = order, Message = "Order created", isSuccess = true };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<UserEntity>> AssignDeliveryToOrder(Guid orderId, Guid userId)
    {
        try
        {
            //check order
            var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null)
            {
                return new ServiceResponse<UserEntity>() { Message = "Order not found" };
            }

            //check delivery person
            var deliveryPerson = await context.Users.FirstOrDefaultAsync(x => x.ID == userId);
            if (deliveryPerson == null)
            {
                return new ServiceResponse<UserEntity>() { Message = "Delivery Person not found" };
            }

            //assign order to delivery person
            order.DeliveryPersonId = deliveryPerson.ID;
            order.DeliveryPerson = deliveryPerson;
            
            order.Status = OrderStatusEnumId.Delivering;
            
            //TODO: if you have time, add push notification to the user.. notifying them of the order being delivered.
            context.Orders.Update(order);
            await context.SaveChangesAsync();

            return new ServiceResponse<UserEntity>()
                { Data = deliveryPerson, Message = "Order assigned", isSuccess = true };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<OrderEntity>> ChangeOrderStatus(Guid orderId, OrderStatusEnumId statudId)
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
                .Include(orderEntity => orderEntity.DeliveryPerson)
                .AsNoTracking()
                .ToListAsync(); // Execute the query and retrieve the data

            // Now process the data in memory
            var result = orders.Select(x => new GetOrderCustomerDto()
            {
                Id = x.Id,
                Status = x.Status.ToString(),
                DeliveryPerson = x.DeliveryPerson == null ? null : new GetDriverDto()
                {
                    FullName = x.DeliveryPerson.FullName,
                    ProfilePicturePath = x.DeliveryPerson.ProfilePicturePath,
                },
                
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
                            ? oi.MenuItem.ImagesOfTheFood.Split(',').ToList() 
                            : [],
                    },
                }).ToList(),
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

    public async Task<ServiceResponse<Guid>> CancelOrder(Guid orderId)
    {
        try
        {
            var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null)
            {
                return new ServiceResponse<Guid>() { Message = "Order not found" };
            }
            order.Status = OrderStatusEnumId.Cancelled;
            context.Orders.Update(order);
            await context.SaveChangesAsync();
            return new ServiceResponse<Guid>() { Data = order.Id, Message = "Order cancelled", isSuccess = true };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ServiceResponse<string>> UpdateDriverCoordinates(Guid orderId, string longitude, string latitude)
    {
        var order = await context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == orderId);
        if (order == null)
        {
            return new ServiceResponse<string>() { Message = "Order not found" };
        }
        var tracking = await context.DeliveryTrackings.Where(x => x.OrderId == orderId).FirstOrDefaultAsync();
        if (tracking == null)
        {
            var newTracking = new DeliveryTrackingEntity()
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ModifiedOn = DateTime.Now,
                CreatedOn = DateTime.Now,
                Latitude = latitude,
                Longitude = longitude,
                Status = true,
                Order = order,
            };
            await context.DeliveryTrackings.AddAsync(newTracking);
            await context.SaveChangesAsync();

            return new ServiceResponse<string>()
            {
                Message = "Tracking updated",
                isSuccess = true,
            };
        }
        
        tracking.Longitude = longitude;
        tracking.Latitude = latitude;
        
        context.DeliveryTrackings.Update(tracking);
        await context.SaveChangesAsync();

        return new ServiceResponse<string>()
        {
            Message = "Tracking updated",
            isSuccess = true,
        };
    }
}