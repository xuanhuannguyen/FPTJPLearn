using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using JPLearn.Api.Extensions;
using JPLearn.Api.Infrastructure.Auth;
using JPLearn.Api.Infrastructure.Security;
using JPLearn.Core.Common.Services;
using JPLearn.Infrastructure.Data;
using JPLearn.Infrastructure.Data.Seed;
using JPLearn.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

// Disable config reload watchers before the default builder adds JSON config files.
// Containers don't need runtime config hot-reload, and FileSystemWatcher can exhaust inotify.
Environment.SetEnvironmentVariable("DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE", "false");
Environment.SetEnvironmentVariable("ASPNETCORE_HOSTBUILDER__RELOADCONFIGONCHANGE", "false");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJPLearnInfrastructure(builder.Configuration);
builder.Services.AddFrontendCors(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.AddJPLearnRateLimiting();
builder.Services.AddHttpContextAccessor();

// Firebase Auth — use FirebaseCurrentUserContext in prod, DevelopmentCurrentUserContext as fallback
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<ICurrentUserContext, DevelopmentCurrentUserContext>();
}
else
{
    builder.Services.AddScoped<ICurrentUserContext, FirebaseCurrentUserContext>();
}

// Initialize Firebase Admin SDK (Chỉ chạy khi không phải tool EF)
if (FirebaseApp.DefaultInstance == null)
{
    try 
    {
        FirebaseApp.Create(new AppOptions
        {
            ProjectId = "jpd-eacda"
        });
    }
    catch (Exception) { /* Bỏ qua lỗi nếu thiếu credential khi chạy tool */ }
}

// === Controllers + Swagger ===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === Middleware Pipeline ===
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Security Headers (chống XSS, Clickjacking)
app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseCors("AllowFrontend");

// Rate Limiting (chống brute-force, spam)
app.UseRateLimiter();

// Firebase auth middleware — runs before controllers
app.UseMiddleware<FirebaseAuthMiddleware>();

app.MapControllers();

// Health check endpoint for Render "Keep-Awake" trick
app.MapGet("/api/health", () => Results.Ok(new { status = "Healthy", time = DateTime.UtcNow }));

// Auto-migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try 
    {
        await db.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseMigration");
        logger.LogError(ex, "Database migration failed on startup.");
    }
}



app.Run();
