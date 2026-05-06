using Microsoft.EntityFrameworkCore;
using JPLearn.Infrastructure.Data;
using JPLearn.Infrastructure.Services;
using JPLearn.Core.Vocabulary;
using JPLearn.Core.Review;

var builder = WebApplication.CreateBuilder(args);

// === Database ===
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// === CORS (cho React frontend) ===
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod();

        if (builder.Environment.IsDevelopment())
        {
            policy.SetIsOriginAllowed(IsLocalFrontendOrigin);
        }
        else
        {
            var allowedOrigins = builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>()
                ?? ["http://localhost:5173"];

            policy.WithOrigins(allowedOrigins);
        }
    });
});

// === Services (DI) ===
builder.Services.AddScoped<IVocabularyService, VocabularyService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

// === Controllers + Swagger ===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === Middleware Pipeline ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.MapControllers();

app.Run();

static bool IsLocalFrontendOrigin(string origin)
{
    if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
    {
        return false;
    }

    return (uri.Scheme is "http" or "https")
        && (uri.Host is "localhost" or "127.0.0.1")
        && uri.Port is >= 5173 and <= 5179;
}
