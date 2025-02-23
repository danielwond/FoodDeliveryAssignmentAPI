using FoodDelivery.Services.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<PaymentEntity> Payments { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<DeliveryTrackingEntity> DeliveryTrackings { get; set; }
    public DbSet<MenuItemEntity> Menus { get; set; }
}