using FoodDelivery.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.DataAccess.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<Entities.UserEntity> Users { get; set; }
    public DbSet<Entities.PaymentEntity> Payments { get; set; }
    public DbSet<Entities.OrderItemEntity> OrderItems { get; set; }
    public DbSet<Entities.DeliveryTrackingEntity> DeliveryTrackings { get; set; }
    public DbSet<Entities.MenuItemEntity> Menus { get; set; }
}