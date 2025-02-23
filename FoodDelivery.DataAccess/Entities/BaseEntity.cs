namespace FoodDelivery.DataAccess.Entities;

public class BaseEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public bool Status { get; set; }
}