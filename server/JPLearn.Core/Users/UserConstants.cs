namespace JPLearn.Core.Users;

public static class UserRoles
{
    public const string Student = "student";
    public const string Admin = "admin";

    public static readonly string[] All = [Student, Admin];

    public static bool IsValid(string? role)
    {
        return !string.IsNullOrWhiteSpace(role)
            && All.Contains(role.Trim().ToLowerInvariant());
    }
}
