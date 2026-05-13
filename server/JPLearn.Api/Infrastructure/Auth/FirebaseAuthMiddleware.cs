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
                string? firebaseUid = null;
                string? email = null;
                string? name = null;

                // Try Firebase SDK first
                if (FirebaseAuth.DefaultInstance != null)
                {
                    try
                    {
                        var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                        firebaseUid = decodedToken.Uid;
                        email = decodedToken.Claims.GetValueOrDefault("email")?.ToString();
                        name = decodedToken.Claims.GetValueOrDefault("name")?.ToString();
                    }
                    catch (Exception)
                    {
                        // Fallback to manual JWT decoding
                    }
                }

                // If SDK failed or isn't initialized, decode JWT manually
                if (string.IsNullOrEmpty(firebaseUid))
                {
                    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        var jwtToken = handler.ReadJwtToken(token);
                        firebaseUid = jwtToken.Claims.FirstOrDefault(c => c.Type == "user_id" || c.Type == "sub")?.Value;
                        email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                        name = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                    }
                }

                if (!string.IsNullOrEmpty(firebaseUid))
                {
                    var userGuid = ConvertToGuid(firebaseUid);

                    // Device session check (1 session limit)
                    var deviceToken = context.Request.Headers["X-Device-Token"].ToString();
                    var isSyncEndpoint = context.Request.Path.StartsWithSegments("/api/auth/sync");

                    if (!string.IsNullOrEmpty(deviceToken) && !isSyncEndpoint)
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
                    context.Items["FirebaseEmail"] = email;
                    context.Items["FirebaseName"] = name;
                }
            }
            catch (Exception)
            {
                // Unhandled parsing error — continue without auth
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
