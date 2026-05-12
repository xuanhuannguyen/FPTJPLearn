using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using JPLearn.Api.Extensions;
using JPLearn.Api.Infrastructure.Auth;
using JPLearn.Core.Common.Services;
using JPLearn.Infrastructure.Data;
using JPLearn.Infrastructure.Data.Seed;
using JPLearn.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJPLearnInfrastructure(builder.Configuration);
builder.Services.AddFrontendCors(builder.Configuration, builder.Environment.IsDevelopment());
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

// Initialize Firebase Admin SDK (no service account needed for token verification only)
if (FirebaseApp.DefaultInstance == null)
{
    FirebaseApp.Create(new AppOptions
    {
        ProjectId = "jpd-eacda"
    });
}

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

// Firebase auth middleware — runs before controllers
app.UseMiddleware<FirebaseAuthMiddleware>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await KanjiSeedData.SeedAsync(db);
    await VocabularySeedData.SeedAsync(db);
    await GrammarSeedData.SeedAsync(db);
    await ExamPracticeSeedData.SeedAsync(db);
    await SpeakingSeedData.SeedAsync(db);
}

app.Run();
