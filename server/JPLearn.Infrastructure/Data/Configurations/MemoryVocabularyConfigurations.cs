using JPLearn.Core.Memory.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JPLearn.Infrastructure.Data.Configurations;

public class UserMemoryVocabularyItemConfiguration : IEntityTypeConfiguration<UserMemoryVocabularyItem>
{
    public void Configure(EntityTypeBuilder<UserMemoryVocabularyItem> builder)
    {
        builder.ToTable("user_memory_vocabulary_items");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Word).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Reading).IsRequired().HasMaxLength(200);
        builder.Property(x => x.WordType).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Meaning).IsRequired().HasMaxLength(500);
        builder.Property(x => x.ExampleJapanese).HasMaxLength(1000);
        builder.Property(x => x.ExampleReading).HasMaxLength(1000);
        builder.Property(x => x.ExampleMeaning).HasMaxLength(1000);
        builder.Property(x => x.Notes).HasMaxLength(1000);
        builder.Property(x => x.CourseCode).HasMaxLength(20);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(20).HasDefaultValue("new");
        builder.Property(x => x.EaseFactor).HasDefaultValue(2.5);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.AddedAt).HasDefaultValueSql("NOW()");

        builder.HasIndex(x => new { x.UserId, x.SourceVocabularyItemId }).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.IsActive, x.NextReviewAt });
        builder.HasIndex(x => new { x.UserId, x.Status });
    }
}
