using JPLearn.Core.Review;
using JPLearn.Core.Admin.Exam;
using JPLearn.Core.ExamPractice;
using JPLearn.Core.Grammar;
using JPLearn.Core.Kanji;
using JPLearn.Core.Memory;
using JPLearn.Core.Payments;
using JPLearn.Core.Speaking;
using JPLearn.Core.StaticVocabulary;
using JPLearn.Core.Settings;
using JPLearn.Core.Vocabulary;
using JPLearn.Infrastructure.Data;
using JPLearn.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JPLearn.Core.Orders;
using JPLearn.Infrastructure.Services.Payments;


namespace JPLearn.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJPLearnInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Tự động chuyển đổi từ format URI (postgresql://...) sang format Npgsql (Host=...;Database=...)
        if (connectionString != null && connectionString.StartsWith("postgresql://"))
        {
            var uri = new Uri(connectionString);
            var userInfo = uri.UserInfo.Split(':');
            var user = userInfo[0];
            var password = userInfo.Length > 1 ? userInfo[1] : "";
            var host = uri.Host;
            var port = uri.Port > 0 ? uri.Port : 5432;
            var database = uri.AbsolutePath.TrimStart('/');

            connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IVocabularyService, VocabularyService>();
        services.AddScoped<IStaticVocabularyService, StaticVocabularyService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IGrammarService, GrammarService>();
        services.AddScoped<IGrammarReviewService, GrammarReviewService>();
        services.AddScoped<IGrammarExerciseService, GrammarExerciseService>();
        services.AddScoped<IKanjiService, KanjiService>();
        services.AddScoped<IMemoryService, MemoryService>();
        services.AddScoped<IMemoryGrammarService, MemoryGrammarService>();
        services.AddScoped<IMemoryKanjiService, MemoryKanjiService>();
        services.AddScoped<IMemoryVocabularyService, MemoryVocabularyService>();
        services.AddScoped<IMemorySrsService, MemorySrsService>();
        services.AddScoped<IAccessSettingsService, AccessSettingsService>();
        services.AddScoped<IPaymentAccessService, PaymentAccessService>();
        services.AddScoped<IExamPracticeService, ExamPracticeService>();
        services.AddScoped<IAdminExamQuestionService, AdminExamQuestionService>();
        services.AddScoped<ISpeakingService, SpeakingService>();
        
        // Payment Providers
        services.AddScoped<IPaymentProvider, SePayProvider>();
        services.AddScoped<IPaymentProvider, PayOSProvider>();


        return services;
    }
}
