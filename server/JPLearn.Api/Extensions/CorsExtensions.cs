namespace JPLearn.Api.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddFrontendCors(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.AllowAnyHeader()
                    .AllowAnyMethod();

                if (isDevelopment)
                {
                    policy.SetIsOriginAllowed(IsLocalFrontendOrigin);
                    return;
                }

                var allowedOrigins = configuration
                    .GetSection("Cors:AllowedOrigins")
                    .Get<string[]>()
                    ?? ["http://localhost:5173"];

                policy.WithOrigins(allowedOrigins);
            });
        });

        return services;
    }

    private static bool IsLocalFrontendOrigin(string origin)
    {
        if (string.IsNullOrEmpty(origin)) return false;

        if (origin.Contains("pinggy-free.link") || origin.Contains("ngrok-free.app") || origin.Contains("run.place"))
        {
            return true;
        }

        if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
        {
            return false;
        }

        return (uri.Scheme is "http" or "https")
            && (uri.Host is "localhost" or "127.0.0.1")
            && uri.Port is >= 3000 and <= 7000; // Increased port range
    }
}
