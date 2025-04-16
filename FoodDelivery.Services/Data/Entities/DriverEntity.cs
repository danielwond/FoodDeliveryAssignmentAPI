namespace FoodDelivery.Services.Data.Entities;

public class DriverEntity
{
    public Guid ID { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string Longitude { get; set; }
    public string Latitude { get; set; }
    public string ProfilePicturePath { get; set; }
    public bool DriverActive { get; set; }
}