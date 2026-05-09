using JPLearn.Core.Memory.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JPLearn.Infrastructure.Data.Configurations;

public class UserMemoryGrammarItemConfiguration : IEntityTypeConfiguration<UserMemoryGrammarItem>
{
    public void Configure(EntityTypeBuilder<UserMemoryGrammarItem> builder)
    {
        builder.ToTable("user_memory_grammar_items");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SourceVersion).HasMaxLength(50);
        builder.Property(x => x.Pattern).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(250);
        builder.Property(x => x.Meaning).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Structure).IsRequired().HasMaxLength(500);
        builder.Property(x => x.UsageScope).HasMaxLength(2000);
        builder.Property(x => x.Formation).HasMaxLength(2000);
        builder.Property(x => x.ExampleJapanese).HasMaxLength(1000);
        builder.Property(x => x.ExampleReading).HasMaxLength(1000);
        builder.Property(x => x.ExampleMeaning).HasMaxLength(1000);
        builder.Property(x => x.Notes).HasMaxLength(4000);
        builder.Property(x => x.TagsJson).HasColumnType("text");
        builder.Property(x => x.Level).HasDefaultValue(0);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(20).HasDefaultValue("new");
        builder.Property(x => x.EaseFactor).HasDefaultValue(2.5);
        builder.Property(x => x.IntervalMinutes).HasDefaultValue(0);
        builder.Property(x => x.IntervalDays).HasDefaultValue(0);
        builder.Property(x => x.LapseCount).HasDefaultValue(0);
        builder.Property(x => x.LearningStepIndex).HasDefaultValue(0);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => new { x.UserId, x.SourceGrammarPatternId }).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.IsActive, x.NextReviewAt });
        builder.HasIndex(x => new { x.UserId, x.Level });
    }
}

public class UserMemoryKanjiItemConfiguration : IEntityTypeConfiguration<UserMemoryKanjiItem>
{
    public void Configure(EntityTypeBuilder<UserMemoryKanjiItem> builder)
    {
        builder.ToTable("user_memory_kanji_items");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Character).IsRequired().HasMaxLength(10);
        builder.Property(x => x.HanViet).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Meaning).IsRequired().HasMaxLength(500);
        builder.Property(x => x.KunReading).HasMaxLength(200);
        builder.Property(x => x.OnReading).HasMaxLength(200);
        builder.Property(x => x.Mnemonic).HasMaxLength(2000);
        builder.Property(x => x.KanjiLevel).HasMaxLength(10);
        builder.Property(x => x.Level).HasDefaultValue(0);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(20).HasDefaultValue("new");
        builder.Property(x => x.EaseFactor).HasDefaultValue(2.5);
        builder.Property(x => x.IntervalMinutes).HasDefaultValue(0);
        builder.Property(x => x.IntervalDays).HasDefaultValue(0);
        builder.Property(x => x.LapseCount).HasDefaultValue(0);
        builder.Property(x => x.LearningStepIndex).HasDefaultValue(0);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => new { x.UserId, x.SourceKanjiItemId }).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.IsActive, x.NextReviewAt });
        builder.HasIndex(x => new { x.UserId, x.Level });
    }
}

public class MemoryReviewSessionConfiguration : IEntityTypeConfiguration<MemoryReviewSession>
{
    public void Configure(EntityTypeBuilder<MemoryReviewSession> builder)
    {
        builder.ToTable("memory_review_sessions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ItemType).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Scope).IsRequired().HasMaxLength(30);

        builder.HasIndex(x => new { x.UserId, x.ItemType, x.StartedAt });
    }
}
