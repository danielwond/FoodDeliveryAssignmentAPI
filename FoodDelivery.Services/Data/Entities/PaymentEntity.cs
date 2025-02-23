namespace FoodDelivery.Services.Data.Entities;

public class PaymentEntity
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public string PaymentMethod { get; set; } // "Cash", "Credit Card", "Wallet"
    public decimal Amount { get; set; }
    public DateTime PaymentTime { get; set; }
    public bool IsSuccessful { get; set; }

    public OrderEntity Order { get; set; }
}