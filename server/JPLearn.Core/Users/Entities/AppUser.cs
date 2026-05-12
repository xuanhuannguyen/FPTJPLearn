using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Users.Entities;

public class AppUser : BaseEntity
{
    public string FirebaseUid { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string Role { get; set; } = UserRoles.Student;
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
    
    // Device session lock
    public string? ActiveDeviceToken { get; set; }
}
