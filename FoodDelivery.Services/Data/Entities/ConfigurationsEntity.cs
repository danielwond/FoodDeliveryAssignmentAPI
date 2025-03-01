using FoodDelivery.Shared.Enums;

namespace FoodDelivery.Services.Data.Entities;

public class ConfigurationsEntity
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ConfigurationsEnum ConfigurationsEnum { get; set; }
    public string Value { get; set; }
}