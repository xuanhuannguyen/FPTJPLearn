namespace JPLearn.Api.Infrastructure.Security;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var headers = context.Response.Headers;

        // Chống XSS: Trình duyệt không chạy inline script trái phép
        headers["X-Content-Type-Options"] = "nosniff";

        // Chống Clickjacking: Không cho nhúng site vào iframe
        headers["X-Frame-Options"] = "DENY";

        // Chặn Referrer leak thông tin nhạy cảm
        headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        // Chặn truy cập camera, microphone, geolocation từ bên ngoài
        headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";

        // Xóa header Server để ẩn công nghệ (ASP.NET / Kestrel)
        headers.Remove("Server");
        headers.Remove("X-Powered-By");

        await _next(context);
    }
}
