using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;


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
                // Skip Firebase auth if SDK not initialized (dev mode)
                if (FirebaseAuth.DefaultInstance == null)
                {
                    await _next(context);
                    return;
                }

                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                var firebaseUid = decodedToken.Uid;
                var userGuid = ConvertToGuid(firebaseUid);

                // Device session check (1 session limit)
                var deviceToken = context.Request.Headers["X-Device-Token"].ToString();
                if (!string.IsNullOrEmpty(deviceToken))
                {
                    using var scope = context.RequestServices.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<JPLearn.Infrastructure.Data.AppDbContext>();
                    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userGuid);
                    
                    if (user != null && !string.IsNullOrEmpty(user.ActiveDeviceToken) && user.ActiveDeviceToken != deviceToken)
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsJsonAsync(new { error = "ACCOUNT_LOGGED_IN_ELSEWHERE", message = "Tài khoản đang đăng nhập ở thiết bị khác." });
                        return;
                    }
                }

                context.Items["FirebaseUserId"] = userGuid;
                context.Items["FirebaseUid"] = firebaseUid;
                context.Items["FirebaseEmail"] = decodedToken.Claims.GetValueOrDefault("email")?.ToString();
                context.Items["FirebaseName"] = decodedToken.Claims.GetValueOrDefault("name")?.ToString();
            }
            catch (Exception)
            {
                // Invalid token or Firebase not available — continue without auth
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
