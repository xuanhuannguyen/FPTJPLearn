using Microsoft.EntityFrameworkCore;
using JPLearn.Core.Grammar.Entities;
using JPLearn.Core.Memory.Entities;
using JPLearn.Core.Vocabulary.Entities;
using JPLearn.Core.Review.Entities;

namespace JPLearn.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<VocabularyList> VocabularyLists => Set<VocabularyList>();
    public DbSet<VocabularyItem> VocabularyItems => Set<VocabularyItem>();
    public DbSet<UserWordProgress> UserWordProgress => Set<UserWordProgress>();
    public DbSet<ReviewSession> ReviewSessions => Set<ReviewSession>();
    public DbSet<GrammarLesson> GrammarLessons => Set<GrammarLesson>();
    public DbSet<GrammarPattern> GrammarPatterns => Set<GrammarPattern>();
    public DbSet<GrammarExample> GrammarExamples => Set<GrammarExample>();
    public DbSet<GrammarExercise> GrammarExercises => Set<GrammarExercise>();
    public DbSet<UserGrammarProgress> UserGrammarProgress => Set<UserGrammarProgress>();
    public DbSet<GrammarExerciseAttempt> GrammarExerciseAttempts => Set<GrammarExerciseAttempt>();
    public DbSet<UserMemoryGrammarItem> UserMemoryGrammarItems => Set<UserMemoryGrammarItem>();
    public DbSet<MemoryReviewSession> MemoryReviewSessions => Set<MemoryReviewSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
