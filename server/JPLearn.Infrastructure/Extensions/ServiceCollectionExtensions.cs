using JPLearn.Core.Review;
using JPLearn.Core.Grammar;
using JPLearn.Core.Memory;
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
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IGrammarService, GrammarService>();
        services.AddScoped<IGrammarReviewService, GrammarReviewService>();
        services.AddScoped<IGrammarExerciseService, GrammarExerciseService>();
        services.AddScoped<IMemoryService, MemoryService>();
        services.AddScoped<IMemoryGrammarService, MemoryGrammarService>();
        services.AddScoped<IMemorySrsService, MemorySrsService>();

        return services;
    }
}
