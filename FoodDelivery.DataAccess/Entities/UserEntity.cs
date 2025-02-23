using FoodDelivery.Shared.Enums;

namespace FoodDelivery.DataAccess.Entities;

public class UserEntity : BaseEntity
{
    public Guid ID { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    
    public byte[] PasswordHash { get; set; } 
    public byte[] PasswordSalt { get; set; }
    
    public string PhoneNumber { get; set; }
    public UserRoleEnum UserRole { get; set; }
    public string? ProfilePicturePath { get; set; }
}