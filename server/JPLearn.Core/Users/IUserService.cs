using JPLearn.Core.Users.Entities;

namespace JPLearn.Core.Users;

public interface IUserService
{
    Task<AppUser?> GetByIdAsync(Guid userId);
    Task<AppUser?> GetByEmailAsync(string email);
    Task<AppUser> CreateAsync(string email, string displayName, string passwordHash);
    Task<bool> ValidatePasswordAsync(string email, string password);
    Task UpdateLastLoginAsync(Guid userId);
}
