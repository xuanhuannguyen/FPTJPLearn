using JPLearn.Core.Payments;
using Microsoft.Extensions.Configuration;

namespace JPLearn.Infrastructure.Services;

public class PaymentAccessService : IPaymentAccessService
{
    private readonly HashSet<string> _unlockedPackageCodes;

    public PaymentAccessService(IConfiguration configuration)
    {
        _unlockedPackageCodes = configuration
            .GetSection("Payments:UnlockedPackageCodes")
            .GetChildren()
            .Select(item => item.Value)
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Select(item => item!.Trim().ToLowerInvariant())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public bool HasContentAccess(Guid userId, string? accessTier, string? packageCode)
    {
        _ = userId;

        var normalizedTier = string.IsNullOrWhiteSpace(accessTier)
            ? PaymentAccessTiers.Free
            : accessTier.Trim().ToLowerInvariant();

        if (normalizedTier == PaymentAccessTiers.Free)
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(packageCode))
        {
            return false;
        }

        return _unlockedPackageCodes.Contains(packageCode.Trim().ToLowerInvariant());
    }
}
