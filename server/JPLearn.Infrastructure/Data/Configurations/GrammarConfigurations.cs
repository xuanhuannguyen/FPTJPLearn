using JPLearn.Core.Grammar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JPLearn.Infrastructure.Data.Configurations;

public class GrammarLessonConfiguration : IEntityTypeConfiguration<GrammarLesson>
{
    public void Configure(EntityTypeBuilder<GrammarLesson> builder)
    {
        builder.ToTable("grammar_lessons");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Level).IsRequired().HasMaxLength(2);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.AccessTier).IsRequired().HasMaxLength(20).HasDefaultValue("free");
        builder.Property(x => x.PackageCode).HasMaxLength(100);

        builder.HasIndex(x => new { x.Level, x.LessonNumber }).IsUnique();
        builder.HasIndex(x => new { x.Level, x.OrderIndex });

        builder.HasMany(x => x.Patterns)
            .WithOne(x => x.Lesson)
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class GrammarPatternConfiguration : IEntityTypeConfiguration<GrammarPattern>
{
    public void Configure(EntityTypeBuilder<GrammarPattern> builder)
    {
        builder.ToTable("grammar_patterns");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Level).IsRequired().HasMaxLength(2);
        builder.Property(x => x.Pattern).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(250);
        builder.Property(x => x.Meaning).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Structure).IsRequired().HasMaxLength(500);
        builder.Property(x => x.UsageScope).HasMaxLength(2000);
        builder.Property(x => x.Formation).HasMaxLength(2000);
        builder.Property(x => x.Notes).HasMaxLength(4000);
        builder.Property(x => x.TagsJson).HasColumnType("text");
        builder.Property(x => x.AccessTierOverride).HasMaxLength(20);
        builder.Property(x => x.PackageCodeOverride).HasMaxLength(100);

        builder.HasIndex(x => x.LessonId);
        builder.HasIndex(x => new { x.Level, x.OrderIndex });
        builder.HasIndex(x => new { x.LessonId, x.Pattern }).IsUnique();

        builder.HasMany(x => x.Examples)
            .WithOne(x => x.Pattern)
            .HasForeignKey(x => x.PatternId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Exercises)
            .WithOne(x => x.Pattern)
            .HasForeignKey(x => x.PatternId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ProgressRecords)
            .WithOne(x => x.GrammarPattern)
            .HasForeignKey(x => x.GrammarPatternId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class GrammarExampleConfiguration : IEntityTypeConfiguration<GrammarExample>
{
    public void Configure(EntityTypeBuilder<GrammarExample> builder)
    {
        builder.ToTable("grammar_examples");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Japanese).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.Reading).HasMaxLength(1000);
        builder.Property(x => x.Meaning).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.Note).HasMaxLength(1000);

        builder.HasIndex(x => new { x.PatternId, x.OrderIndex });
    }
}

public class GrammarExerciseConfiguration : IEntityTypeConfiguration<GrammarExercise>
{
    public void Configure(EntityTypeBuilder<GrammarExercise> builder)
    {
        builder.ToTable("grammar_exercises");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ExerciseType).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Prompt).IsRequired().HasMaxLength(1500);
        builder.Property(x => x.PromptReading).HasMaxLength(1500);
        builder.Property(x => x.ExpectedAnswer).IsRequired().HasMaxLength(1500);
        builder.Property(x => x.AcceptableAnswersJson).HasColumnType("text");
        builder.Property(x => x.Hint).HasMaxLength(1000);
        builder.Property(x => x.Explanation).HasMaxLength(2000);
        builder.Property(x => x.TemplateText).HasMaxLength(1500);
        builder.Property(x => x.OptionsJson).HasColumnType("text");
        builder.Property(x => x.CorrectOrderJson).HasColumnType("text");
        builder.Property(x => x.StarAnswer).HasMaxLength(500);

        builder.HasIndex(x => new { x.PatternId, x.ExerciseType, x.OrderIndex });

        builder.HasMany(x => x.Attempts)
            .WithOne(x => x.GrammarExercise)
            .HasForeignKey(x => x.GrammarExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class UserGrammarProgressConfiguration : IEntityTypeConfiguration<UserGrammarProgress>
{
    public void Configure(EntityTypeBuilder<UserGrammarProgress> builder)
    {
        builder.ToTable("user_grammar_progress");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Level).HasDefaultValue(0);
        builder.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("new");
        builder.Property(x => x.EaseFactor).HasDefaultValue(2.5);
        builder.Property(x => x.LapseCount).HasDefaultValue(0);
        builder.Property(x => x.LearningStepIndex).HasDefaultValue(0);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => new { x.UserId, x.GrammarPatternId }).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.IsActive, x.NextReviewAt });
    }
}

public class GrammarExerciseAttemptConfiguration : IEntityTypeConfiguration<GrammarExerciseAttempt>
{
    public void Configure(EntityTypeBuilder<GrammarExerciseAttempt> builder)
    {
        builder.ToTable("grammar_exercise_attempts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AnswerText).HasMaxLength(2000);
        builder.Property(x => x.SelectedOptionOrderJson).HasColumnType("text");
        builder.Property(x => x.Feedback).HasMaxLength(4000);
        builder.Property(x => x.CheckedBy).IsRequired().HasMaxLength(20).HasDefaultValue("system");

        builder.HasIndex(x => new { x.UserId, x.GrammarExerciseId, x.CreatedAt });
    }
}
