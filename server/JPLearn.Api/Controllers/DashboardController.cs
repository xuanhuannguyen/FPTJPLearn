using JPLearn.Core.Orders.Entities;
using JPLearn.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("trust-summary")]
    public async Task<IActionResult> GetTrustSummary()
    {
        var activeUsers = await _db.Users.CountAsync(user => user.IsActive);
        var paidLearners = await _db.Orders
            .Where(order => order.Status == OrderStatuses.Paid)
            .Select(order => order.UserId)
            .Distinct()
            .CountAsync();

        var contentItems =
            await _db.StaticVocabularyLessons.CountAsync() +
            await _db.StaticVocabularyItems.CountAsync() +
            await _db.GrammarLessons.CountAsync() +
            await _db.GrammarPatterns.CountAsync() +
            await _db.KanjiLessons.CountAsync() +
            await _db.KanjiItems.CountAsync() +
            await _db.ExamQuestions.CountAsync() +
            await _db.SpeakingLessons.CountAsync() +
            await _db.SpeakingSentences.CountAsync();

        var recentBuyers = await _db.Orders
            .Where(order => order.Status == OrderStatuses.Paid)
            .Join(
                _db.Users,
                order => order.UserId,
                user => user.Id,
                (order, user) => new
                {
                    user.Email,
                    order.PackageCode,
                    order.PaidAt,
                    order.CreatedAt
                })
            .OrderByDescending(order => order.PaidAt ?? order.CreatedAt)
            .Take(3)
            .ToListAsync();

        return Ok(new
        {
            activeUsers,
            paidLearners,
            contentItems,
            recentBuyers = recentBuyers.Select(order => new
            {
                buyer = MaskEmail(order.Email),
                packageName = GetPackageName(order.PackageCode),
                time = FormatRelativeTime(order.PaidAt ?? order.CreatedAt)
            })
        });
    }

    private static string MaskEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return "học viên JPLearn";
        }

        var atIndex = email.IndexOf('@');
        if (atIndex <= 0)
        {
            return $"{email[0]}***";
        }

        return $"{email[0]}***{email[atIndex..]}";
    }

    private static string GetPackageName(string packageCode)
    {
        return packageCode.ToLowerInvariant() switch
        {
            PackageCodes.Combo => "Combo JPD113 + JPD123",
            PackageCodes.JPD113 => "Gói JPD113",
            PackageCodes.JPD123 => "Gói JPD123",
            PackageCodes.JPD133 => "Gói JPD133",
            _ => packageCode.ToUpperInvariant()
        };
    }

    private static string FormatRelativeTime(DateTime dateTime)
    {
        var days = (DateTime.UtcNow.Date - dateTime.Date).Days;

        return days switch
        {
            <= 0 => "Hôm nay",
            1 => "Hôm qua",
            _ when days < 7 => $"{days} ngày trước",
            _ => dateTime.ToLocalTime().ToString("dd/MM/yyyy")
        };
    }
}
