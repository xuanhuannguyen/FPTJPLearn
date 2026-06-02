using JPLearn.Core.Payments;
using JPLearn.Core.Settings;
using JPLearn.Infrastructure.Data;

namespace JPLearn.Infrastructure.Services;

public class PaymentAccessService : IPaymentAccessService
{
    private readonly AppDbContext _db;
    private readonly IAccessSettingsService _accessSettings;

    public PaymentAccessService(AppDbContext db, IAccessSettingsService accessSettings)
    {
        _db = db;
        _accessSettings = accessSettings;
    }

    public bool HasContentAccess(Guid userId, string? accessTier, string? packageCode)
    {
        if (IsFreeExperienceEnabled())
            return true;

        var normalizedTier = string.IsNullOrWhiteSpace(accessTier)
            ? PaymentAccessTiers.Free
            : accessTier.Trim().ToLowerInvariant();

        // Free content = always accessible
        if (normalizedTier == PaymentAccessTiers.Free)
            return true;

        // Guest = no premium
        if (userId == Guid.Empty) return false;

        // Map lesson packageCode → subscription courseCode
        // Lesson seeds use: kanji_jpd113, vocab_jpd113, grammar_jpd113, etc.
        // Subscriptions use: jpd113, jpd123
        var courseCode = MapToCourseCode(packageCode);
        if (string.IsNullOrEmpty(courseCode)) return false;

        return _db.Subscriptions.Any(s =>
            s.UserId == userId &&
            s.CourseCode == courseCode &&
            s.ExpiresAt > DateTime.UtcNow);
    }

    /// <summary>
    /// Maps lesson-level packageCode (kanji_jpd113, grammar_jpd123, etc.)
    /// to subscription courseCode (jpd113, jpd123).
    /// </summary>
    private static string? MapToCourseCode(string? packageCode)
    {
        if (string.IsNullOrWhiteSpace(packageCode)) return null;
        var code = packageCode.Trim().ToLowerInvariant();

        if (code.Contains("jpd113")) return "jpd113";
        if (code.Contains("jpd123")) return "jpd123";

        return code;
    }

    private bool IsFreeExperienceEnabled()
    {
        return _accessSettings.IsFreeExperienceEnabled();
    }
}
