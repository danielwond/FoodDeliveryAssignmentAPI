namespace FoodDelivery.Shared.Options;

public class SeedDataOptions
{
    public bool SeedAccounts { get; set; }
    public bool SeedMenus { get; set; }
    public bool SeedConfigurations { get; set; }


    public Dictionary<string, FoodImages> Images { get; set; }
}

public class FoodImages
{
    public string Img1 { get; set; }
    public string Img2 { get; set; }
    public string Img3 { get; set; }
}
