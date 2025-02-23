namespace FoodDelivery.Shared;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public bool isSuccess { get; set; } = false;
}