using FoodDelivery.Services.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Services.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<MenuItemEntity> Menus { get; set; }
    public DbSet<ConfigurationsEntity> Configurations { get; set; }
    public DbSet<DriverEntity> Drivers { get; set; }
}