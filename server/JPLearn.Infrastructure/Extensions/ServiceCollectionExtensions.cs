using JPLearn.Core.Review;
using JPLearn.Core.Admin.Exam;
using JPLearn.Core.ExamPractice;
using JPLearn.Core.Grammar;
using JPLearn.Core.Kanji;
using JPLearn.Core.Memory;
using JPLearn.Core.Payments;
using JPLearn.Core.Speaking;
using JPLearn.Core.StaticVocabulary;
using JPLearn.Core.Vocabulary;
using JPLearn.Infrastructure.Data;
using JPLearn.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JPLearn.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJPLearnInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

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
<<<<<<< HEAD
        services.AddScoped<IMemoryVocabularyService, MemoryVocabularyService>();
=======
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
        services.AddScoped<IMemorySrsService, MemorySrsService>();
        services.AddScoped<IPaymentAccessService, PaymentAccessService>();
        services.AddScoped<IExamPracticeService, ExamPracticeService>();
        services.AddScoped<IAdminExamQuestionService, AdminExamQuestionService>();
        services.AddScoped<ISpeakingService, SpeakingService>();

        return services;
    }
}
