namespace JPLearn.Core.Payments;

public interface IPaymentAccessService
{
    bool HasContentAccess(Guid userId, string? accessTier, string? packageCode);
    bool IsContentLocked(Guid userId, string? accessTier, string? packageCode)
    {
        return !HasContentAccess(userId, accessTier, packageCode);
    }
}
