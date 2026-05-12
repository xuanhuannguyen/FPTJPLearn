using System.Security.Claims;
using JPLearn.Core.Common.Services;
using FirebaseAdmin;
using FirebaseAdmin.Auth;

namespace JPLearn.Api.Infrastructure.Auth;

public class FirebaseCurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FirebaseCurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => ResolveUserId();

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.Items["FirebaseUserId"] != null;

    private Guid ResolveUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return DevelopmentCurrentUserContext.FallbackUserId;
        }

        // Firebase UID stored by middleware
        if (httpContext.Items["FirebaseUserId"] is Guid firebaseGuid)
        {
            return firebaseGuid;
        }

        // Fallback to header for dev/testing
        if (httpContext.Request.Headers.TryGetValue("X-User-Id", out var headerValues)
            && Guid.TryParse(headerValues.FirstOrDefault(), out var headerUserId))
        {
            return headerUserId;
        }

        return DevelopmentCurrentUserContext.FallbackUserId;
    }
}
