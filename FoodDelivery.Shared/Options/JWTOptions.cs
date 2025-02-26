namespace FoodDelivery.Shared.Options;

public class JWTOptions
{
    public string Secret { get; set; }
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
}