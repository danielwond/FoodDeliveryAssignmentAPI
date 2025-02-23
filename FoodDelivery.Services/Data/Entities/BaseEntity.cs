namespace FoodDelivery.Services.Data.Entities;

public class BaseEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public bool Status { get; set; }
}