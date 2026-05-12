using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JPLearn.Core.Vocabulary.Entities;

namespace JPLearn.Infrastructure.Data.Configurations;

public class VocabularyListConfiguration : IEntityTypeConfiguration<VocabularyList>
{
    public void Configure(EntityTypeBuilder<VocabularyList> builder)
    {
        builder.ToTable("active_vocabulary_lists");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.HasIndex(x => x.UserId);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.List)
            .HasForeignKey(x => x.ListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class VocabularyItemConfiguration : IEntityTypeConfiguration<VocabularyItem>
{
    public void Configure(EntityTypeBuilder<VocabularyItem> builder)
    {
        builder.ToTable("active_vocabulary_items");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Word).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Reading).IsRequired().HasMaxLength(100);
        builder.Property(x => x.WordType).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Meaning).IsRequired().HasMaxLength(200);
        builder.HasIndex(x => x.ListId);
    }
}

public class UserWordProgressConfiguration : IEntityTypeConfiguration<UserWordProgress>
{
    public void Configure(EntityTypeBuilder<UserWordProgress> builder)
    {
        builder.ToTable("user_active_word_progress");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Level).HasDefaultValue(0);
        builder.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("new");
        builder.Property(x => x.EaseFactor).HasDefaultValue(2.5);
        builder.Property(x => x.LapseCount).HasDefaultValue(0);
        builder.Property(x => x.LearningStepIndex).HasDefaultValue(0);

        builder.HasIndex(x => new { x.UserId, x.VocabularyItemId }).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.NextReviewAt });

        builder.HasOne(x => x.VocabularyItem)
            .WithMany(x => x.ProgressRecords)
            .HasForeignKey(x => x.VocabularyItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
