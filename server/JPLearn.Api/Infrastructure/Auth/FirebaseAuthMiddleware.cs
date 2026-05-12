using FirebaseAdmin;
using FirebaseAdmin.Auth;

namespace JPLearn.Api.Infrastructure.Auth;

public class FirebaseAuthMiddleware
{
    private readonly RequestDelegate _next;

    public FirebaseAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader["Bearer ".Length..];
            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                var firebaseUid = decodedToken.Uid;

                // Convert Firebase UID (string) to deterministic Guid
                var userGuid = ConvertToGuid(firebaseUid);
                context.Items["FirebaseUserId"] = userGuid;
                context.Items["FirebaseUid"] = firebaseUid;
                context.Items["FirebaseEmail"] = decodedToken.Claims.GetValueOrDefault("email")?.ToString();
                context.Items["FirebaseName"] = decodedToken.Claims.GetValueOrDefault("name")?.ToString();
            }
            catch (FirebaseAuthException)
            {
                // Invalid token — continue without auth (endpoints will use fallback userId)
            }
        }

        await _next(context);
    }

    private static Guid ConvertToGuid(string firebaseUid)
    {
        // Deterministic: same Firebase UID always produces same Guid
        using var md5 = System.Security.Cryptography.MD5.Create();
        var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(firebaseUid));
        return new Guid(hash);
    }
}
