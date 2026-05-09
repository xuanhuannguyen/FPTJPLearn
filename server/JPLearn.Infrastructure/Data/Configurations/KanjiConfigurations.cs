using JPLearn.Core.Kanji.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JPLearn.Infrastructure.Data.Configurations;

public class KanjiLessonConfiguration : IEntityTypeConfiguration<KanjiLesson>
{
    public void Configure(EntityTypeBuilder<KanjiLesson> builder)
    {
        builder.ToTable("kanji_lessons");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Level).IsRequired().HasMaxLength(2);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.AccessTier).IsRequired().HasMaxLength(20).HasDefaultValue("free");
        builder.Property(x => x.PackageCode).HasMaxLength(100);

        builder.HasIndex(x => new { x.Level, x.LessonNumber }).IsUnique();
        builder.HasIndex(x => new { x.Level, x.OrderIndex });

        builder.HasMany(x => x.KanjiItems)
            .WithOne(x => x.Lesson)
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.VocabularyItems)
            .WithOne(x => x.Lesson)
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class KanjiItemConfiguration : IEntityTypeConfiguration<KanjiItem>
{
    public void Configure(EntityTypeBuilder<KanjiItem> builder)
    {
        builder.ToTable("kanji_items");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Level).IsRequired().HasMaxLength(2);
        builder.Property(x => x.Character).IsRequired().HasMaxLength(8);
        builder.Property(x => x.HanViet).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Meaning).IsRequired().HasMaxLength(300);
        builder.Property(x => x.KunReading).HasMaxLength(300);
        builder.Property(x => x.OnReading).HasMaxLength(300);
        builder.Property(x => x.Mnemonic).HasMaxLength(1000);
        builder.Property(x => x.StrokeSvg).HasColumnType("text");
        builder.Property(x => x.StrokeDataJson).HasColumnType("text");
        builder.Property(x => x.ComponentMapJson).HasColumnType("text");
        builder.Property(x => x.AccessTierOverride).HasMaxLength(20);
        builder.Property(x => x.PackageCodeOverride).HasMaxLength(100);

        builder.HasIndex(x => x.Character).IsUnique();
        builder.HasIndex(x => x.LessonId);
        builder.HasIndex(x => new { x.Level, x.OrderIndex });

        builder.HasMany(x => x.VocabularyItems)
            .WithOne(x => x.KanjiItem)
            .HasForeignKey(x => x.KanjiItemId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.ProgressRecords)
            .WithOne(x => x.KanjiItem)
            .HasForeignKey(x => x.KanjiItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class KanjiVocabularyConfiguration : IEntityTypeConfiguration<KanjiVocabulary>
{
    public void Configure(EntityTypeBuilder<KanjiVocabulary> builder)
    {
        builder.ToTable("kanji_vocabulary");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Level).IsRequired().HasMaxLength(2);
        builder.Property(x => x.Word).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Reading).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Meaning).IsRequired().HasMaxLength(500);
        builder.Property(x => x.ExampleJapanese).HasMaxLength(1000);
        builder.Property(x => x.ExampleReading).HasMaxLength(1000);
        builder.Property(x => x.ExampleMeaning).HasMaxLength(1000);

        builder.HasIndex(x => x.LessonId);
        builder.HasIndex(x => x.KanjiItemId);
        builder.HasIndex(x => new { x.LessonId, x.Word }).IsUnique();
        builder.HasIndex(x => new { x.Level, x.OrderIndex });
    }
}

public class UserKanjiProgressConfiguration : IEntityTypeConfiguration<UserKanjiProgress>
{
    public void Configure(EntityTypeBuilder<UserKanjiProgress> builder)
    {
        builder.ToTable("user_kanji_progress");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.IsLearned).HasDefaultValue(false);
        builder.Property(x => x.WritingPracticeCount).HasDefaultValue(0);
        builder.Property(x => x.FlashcardPracticeCount).HasDefaultValue(0);

        builder.HasIndex(x => new { x.UserId, x.KanjiItemId }).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.IsLearned });
        builder.HasIndex(x => new { x.UserId, x.LastViewedAt });
    }
}
