using System.Security.Claims;
using JPLearn.Core.Common.Services;

namespace JPLearn.Api.Infrastructure.Auth;

public class DevelopmentCurrentUserContext : ICurrentUserContext
{
    public static readonly Guid FallbackUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    private readonly IHttpContextAccessor _httpContextAccessor;

    public DevelopmentCurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => ResolveUserId();

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    private Guid ResolveUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return FallbackUserId;
        }

        var claimValue = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? httpContext.User.FindFirstValue("sub")
            ?? httpContext.User.FindFirstValue("user_id");

        if (Guid.TryParse(claimValue, out var claimUserId))
        {
            return claimUserId;
        }

        if (httpContext.Request.Headers.TryGetValue("X-User-Id", out var headerValues)
            && Guid.TryParse(headerValues.FirstOrDefault(), out var headerUserId))
        {
            return headerUserId;
        }

        return FallbackUserId;
    }
}
