using JPLearn.Core.Speaking;
using JPLearn.Core.Speaking.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JPLearn.Infrastructure.Data.Configurations;

public class SpeakingCourseConfiguration : IEntityTypeConfiguration<SpeakingCourse>
{
    public void Configure(EntityTypeBuilder<SpeakingCourse> builder)
    {
        builder.ToTable("speaking_courses");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.AccessTier).IsRequired().HasMaxLength(30).HasDefaultValue(SpeakingAccessTiers.Free);
        builder.Property(x => x.PackageCode).HasMaxLength(100);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => new { x.IsActive, x.OrderIndex });
    }
}

public class SpeakingLessonConfiguration : IEntityTypeConfiguration<SpeakingLesson>
{
    public void Configure(EntityTypeBuilder<SpeakingLesson> builder)
    {
        builder.ToTable("speaking_lessons");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CourseCode).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.AccessTier).IsRequired().HasMaxLength(30).HasDefaultValue(SpeakingAccessTiers.Free);
        builder.Property(x => x.PackageCode).HasMaxLength(100);
        builder.Property(x => x.LessonType).IsRequired().HasMaxLength(30).HasDefaultValue(SpeakingLessonTypes.Reading);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => new { x.CourseCode, x.LessonNumber }).IsUnique();
        builder.HasIndex(x => new { x.CourseCode, x.OrderIndex });

        builder.HasOne(x => x.Course)
            .WithMany(x => x.Lessons)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SpeakingSentenceConfiguration : IEntityTypeConfiguration<SpeakingSentence>
{
    public void Configure(EntityTypeBuilder<SpeakingSentence> builder)
    {
        builder.ToTable("speaking_sentences");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PlainText).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.ContentHtml).IsRequired().HasMaxLength(5000);
        builder.Property(x => x.MeaningVi).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => new { x.LessonId, x.OrderIndex });
        builder.HasIndex(x => new { x.LessonId, x.SentenceNumber }).IsUnique();

        builder.HasOne(x => x.Lesson)
            .WithMany(x => x.Sentences)
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
