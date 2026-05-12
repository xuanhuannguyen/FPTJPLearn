using JPLearn.Core.StaticVocabulary.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JPLearn.Infrastructure.Data.Configurations;

public class VocabularyCourseConfiguration : IEntityTypeConfiguration<VocabularyCourse>
{
    public void Configure(EntityTypeBuilder<VocabularyCourse> builder)
    {
        builder.ToTable("vocabulary_courses");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(1000);

        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.OrderIndex);

        builder.HasMany(x => x.Lessons)
            .WithOne(x => x.Course)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class VocabularyLessonConfiguration : IEntityTypeConfiguration<VocabularyLesson>
{
    public void Configure(EntityTypeBuilder<VocabularyLesson> builder)
    {
        builder.ToTable("vocabulary_lessons");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CourseCode).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.AccessTier).IsRequired().HasMaxLength(20).HasDefaultValue("free");
        builder.Property(x => x.PackageCode).HasMaxLength(100);

        builder.HasIndex(x => x.CourseId);
        builder.HasIndex(x => new { x.CourseCode, x.LessonNumber }).IsUnique();
        builder.HasIndex(x => new { x.CourseCode, x.OrderIndex });

        builder.HasMany(x => x.Items)
            .WithOne(x => x.Lesson)
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class StaticVocabularyItemConfiguration : IEntityTypeConfiguration<StaticVocabularyItem>
{
    public void Configure(EntityTypeBuilder<StaticVocabularyItem> builder)
    {
        builder.ToTable("vocabulary_items");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CourseCode).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Word).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Reading).IsRequired().HasMaxLength(200);
        builder.Property(x => x.WordType).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Meaning).IsRequired().HasMaxLength(500);
        builder.Property(x => x.ExampleJapanese).HasMaxLength(1000);
        builder.Property(x => x.ExampleReading).HasMaxLength(1000);
        builder.Property(x => x.ExampleMeaning).HasMaxLength(1000);
        builder.Property(x => x.Notes).HasMaxLength(1000);
        builder.Property(x => x.AccessTierOverride).HasMaxLength(20);
        builder.Property(x => x.PackageCodeOverride).HasMaxLength(100);

        builder.HasIndex(x => x.LessonId);
        builder.HasIndex(x => new { x.LessonId, x.Word }).IsUnique();
        builder.HasIndex(x => new { x.CourseCode, x.OrderIndex });

        builder.HasMany(x => x.ProgressRecords)
            .WithOne(x => x.VocabularyItem)
            .HasForeignKey(x => x.VocabularyItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class UserVocabularyProgressConfiguration : IEntityTypeConfiguration<UserVocabularyProgress>
{
    public void Configure(EntityTypeBuilder<UserVocabularyProgress> builder)
    {
        builder.ToTable("user_vocabulary_progress");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.IsLearned).HasDefaultValue(false);
        builder.Property(x => x.FlashcardPracticeCount).HasDefaultValue(0);
        builder.Property(x => x.MultipleChoicePracticeCount).HasDefaultValue(0);
        builder.Property(x => x.TypingPracticeCount).HasDefaultValue(0);

        builder.HasIndex(x => new { x.UserId, x.VocabularyItemId }).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.IsLearned });
        builder.HasIndex(x => new { x.UserId, x.LastViewedAt });
    }
}
