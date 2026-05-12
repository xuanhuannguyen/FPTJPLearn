using JPLearn.Core.ExamPractice;
using JPLearn.Core.ExamPractice.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JPLearn.Infrastructure.Data.Configurations;

public class ExamCourseConfiguration : IEntityTypeConfiguration<ExamCourse>
{
    public void Configure(EntityTypeBuilder<ExamCourse> builder)
    {
        builder.ToTable("exam_courses");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.AccessTier).IsRequired().HasMaxLength(30).HasDefaultValue("free");
        builder.Property(x => x.PackageCode).HasMaxLength(100);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasAlternateKey(x => x.Code);
        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => new { x.IsActive, x.OrderIndex });
    }
}

public class ExamTopicConfiguration : IEntityTypeConfiguration<ExamTopic>
{
    public void Configure(EntityTypeBuilder<ExamTopic> builder)
    {
        builder.ToTable("exam_topics");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CourseCode).IsRequired().HasMaxLength(30).HasDefaultValue("jpd113");
        builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Label).IsRequired().HasMaxLength(200);
        builder.Property(x => x.OrderIndex).HasDefaultValue(0);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasAlternateKey(x => new { x.CourseCode, x.Code });
        builder.HasIndex(x => new { x.CourseCode, x.IsActive, x.OrderIndex });

        builder.HasOne(x => x.Course)
            .WithMany()
            .HasForeignKey(x => x.CourseCode)
            .HasPrincipalKey(x => x.Code)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ExamPassageConfiguration : IEntityTypeConfiguration<ExamPassage>
{
    public void Configure(EntityTypeBuilder<ExamPassage> builder)
    {
        builder.ToTable("exam_passages");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CourseCode).IsRequired().HasMaxLength(30).HasDefaultValue("jpd113");
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Content).IsRequired().HasMaxLength(5000);
        builder.Property(x => x.Level).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Topic).IsRequired().HasMaxLength(50);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => new { x.CourseCode, x.Level, x.Topic, x.IsActive });
        builder.HasIndex(x => x.OrderIndex);

        builder.HasOne(x => x.TopicDefinition)
            .WithMany(x => x.Passages)
            .HasForeignKey(x => new { x.CourseCode, x.Topic })
            .HasPrincipalKey(x => new { x.CourseCode, x.Code })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Course)
            .WithMany(x => x.Passages)
            .HasForeignKey(x => x.CourseCode)
            .HasPrincipalKey(x => x.Code)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Questions)
            .WithOne(x => x.Passage)
            .HasForeignKey(x => x.PassageId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
{
    public void Configure(EntityTypeBuilder<ExamQuestion> builder)
    {
        builder.ToTable("exam_questions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CourseCode).IsRequired().HasMaxLength(30).HasDefaultValue("jpd113");
        builder.Property(x => x.QuestionType).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Topic).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Level).IsRequired().HasMaxLength(10);
        builder.Property(x => x.QuestionText).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.Explanation).IsRequired().HasMaxLength(3000);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => x.PassageId);
        builder.HasIndex(x => new { x.CourseCode, x.Topic, x.Level, x.IsActive });
        builder.HasIndex(x => new { x.CourseCode, x.Level, x.OrderIndex });

        builder.HasOne(x => x.TopicDefinition)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => new { x.CourseCode, x.Topic })
            .HasPrincipalKey(x => new { x.CourseCode, x.Code })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Course)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.CourseCode)
            .HasPrincipalKey(x => x.Code)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Options)
            .WithOne(x => x.Question)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ExamQuestionOptionConfiguration : IEntityTypeConfiguration<ExamQuestionOption>
{
    public void Configure(EntityTypeBuilder<ExamQuestionOption> builder)
    {
        builder.ToTable("exam_question_options");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Label).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Text).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.IsCorrect).HasDefaultValue(false);

        builder.HasIndex(x => x.QuestionId);
        builder.HasIndex(x => new { x.QuestionId, x.Label }).IsUnique();
    }
}

public class ExamAttemptConfiguration : IEntityTypeConfiguration<ExamAttempt>
{
    public void Configure(EntityTypeBuilder<ExamAttempt> builder)
    {
        builder.ToTable("exam_attempts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CourseCode).IsRequired().HasMaxLength(30).HasDefaultValue("jpd113");
        builder.Property(x => x.Status).IsRequired().HasMaxLength(30);
        builder.Property(x => x.StartedAt).HasDefaultValueSql("NOW()");
        builder.Property(x => x.DurationMinutes).HasDefaultValue(30);
        builder.Property(x => x.TotalQuestions).HasDefaultValue(30);
        builder.Property(x => x.ScorePercent).HasDefaultValue(0);
        builder.Property(x => x.Mode).IsRequired().HasMaxLength(30).HasDefaultValue(ExamAttemptModes.Practice);

        builder.HasIndex(x => new { x.UserId, x.CourseCode, x.Status });
        builder.HasIndex(x => new { x.UserId, x.StartedAt });
        builder.HasIndex(x => x.ExpiresAt);
        builder.HasIndex(x => x.BlueprintId);

        builder.HasOne(x => x.Course)
            .WithMany(x => x.Attempts)
            .HasForeignKey(x => x.CourseCode)
            .HasPrincipalKey(x => x.Code)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Blueprint)
            .WithMany()
            .HasForeignKey(x => x.BlueprintId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.Answers)
            .WithOne(x => x.Attempt)
            .HasForeignKey(x => x.AttemptId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ExamAttemptAnswerConfiguration : IEntityTypeConfiguration<ExamAttemptAnswer>
{
    public void Configure(EntityTypeBuilder<ExamAttemptAnswer> builder)
    {
        builder.ToTable("exam_attempt_answers");
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.AttemptId, x.QuestionId }).IsUnique();
        builder.HasIndex(x => new { x.AttemptId, x.SequenceNumber });
        builder.HasIndex(x => x.QuestionId);
        builder.HasIndex(x => x.SelectedOptionId);

        builder.HasOne(x => x.Question)
            .WithMany(x => x.AttemptAnswers)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SelectedOption)
            .WithMany()
            .HasForeignKey(x => x.SelectedOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ExamBlueprintConfiguration : IEntityTypeConfiguration<ExamBlueprint>
{
    public void Configure(EntityTypeBuilder<ExamBlueprint> builder)
    {
        builder.ToTable("exam_blueprints");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CourseCode).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.TimeLimitMinutes).HasDefaultValue(30);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasIndex(x => new { x.CourseCode, x.IsActive });

        builder.HasOne(x => x.Course)
            .WithMany()
            .HasForeignKey(x => x.CourseCode)
            .HasPrincipalKey(x => x.Code)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Rules)
            .WithOne(x => x.Blueprint)
            .HasForeignKey(x => x.BlueprintId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ExamBlueprintRuleConfiguration : IEntityTypeConfiguration<ExamBlueprintRule>
{
    public void Configure(EntityTypeBuilder<ExamBlueprintRule> builder)
    {
        builder.ToTable("exam_blueprint_rules");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Topic).IsRequired().HasMaxLength(50);
        builder.Property(x => x.QuestionCount).HasDefaultValue(0);

        builder.HasIndex(x => x.BlueprintId);
    }
}

public class ExamPracticeProgressConfiguration : IEntityTypeConfiguration<ExamPracticeProgress>
{
    public void Configure(EntityTypeBuilder<ExamPracticeProgress> builder)
    {
        builder.ToTable("exam_practice_progress");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CourseCode).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Topic).IsRequired().HasMaxLength(50);
        builder.Property(x => x.TotalCompleted).HasDefaultValue(0);

        builder.HasIndex(x => new { x.UserId, x.CourseCode, x.Topic }).IsUnique();
    }
}
