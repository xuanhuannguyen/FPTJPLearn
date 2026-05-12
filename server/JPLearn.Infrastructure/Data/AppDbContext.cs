using Microsoft.EntityFrameworkCore;
using JPLearn.Core.Grammar.Entities;
<<<<<<< HEAD
using JPLearn.Core.ExamPractice.Entities;
=======
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
using JPLearn.Core.Kanji.Entities;
using JPLearn.Core.Memory.Entities;
using JPLearn.Core.Speaking.Entities;
using JPLearn.Core.StaticVocabulary.Entities;
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
    public DbSet<KanjiLesson> KanjiLessons => Set<KanjiLesson>();
    public DbSet<KanjiItem> KanjiItems => Set<KanjiItem>();
    public DbSet<KanjiVocabulary> KanjiVocabularyItems => Set<KanjiVocabulary>();
    public DbSet<UserKanjiProgress> UserKanjiProgress => Set<UserKanjiProgress>();
    public DbSet<UserMemoryGrammarItem> UserMemoryGrammarItems => Set<UserMemoryGrammarItem>();
    public DbSet<UserMemoryKanjiItem> UserMemoryKanjiItems => Set<UserMemoryKanjiItem>();
<<<<<<< HEAD
    public DbSet<UserMemoryVocabularyItem> UserMemoryVocabularyItems => Set<UserMemoryVocabularyItem>();
=======
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
    public DbSet<MemoryReviewSession> MemoryReviewSessions => Set<MemoryReviewSession>();
    public DbSet<VocabularyCourse> VocabularyCourses => Set<VocabularyCourse>();
    public DbSet<VocabularyLesson> StaticVocabularyLessons => Set<VocabularyLesson>();
    public DbSet<StaticVocabularyItem> StaticVocabularyItems => Set<StaticVocabularyItem>();
    public DbSet<UserVocabularyProgress> UserVocabularyProgress => Set<UserVocabularyProgress>();
    public DbSet<ExamCourse> ExamCourses => Set<ExamCourse>();
    public DbSet<ExamTopic> ExamTopics => Set<ExamTopic>();
    public DbSet<ExamPassage> ExamPassages => Set<ExamPassage>();
    public DbSet<ExamQuestion> ExamQuestions => Set<ExamQuestion>();
    public DbSet<ExamQuestionOption> ExamQuestionOptions => Set<ExamQuestionOption>();
    public DbSet<ExamBlueprint> ExamBlueprints => Set<ExamBlueprint>();
    public DbSet<ExamBlueprintRule> ExamBlueprintRules => Set<ExamBlueprintRule>();
    public DbSet<ExamPracticeProgress> ExamPracticeProgresses => Set<ExamPracticeProgress>();
    public DbSet<ExamAttempt> ExamAttempts => Set<ExamAttempt>();
    public DbSet<ExamAttemptAnswer> ExamAttemptAnswers => Set<ExamAttemptAnswer>();
    public DbSet<SpeakingCourse> SpeakingCourses => Set<SpeakingCourse>();
    public DbSet<SpeakingLesson> SpeakingLessons => Set<SpeakingLesson>();
    public DbSet<SpeakingSentence> SpeakingSentences => Set<SpeakingSentence>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
