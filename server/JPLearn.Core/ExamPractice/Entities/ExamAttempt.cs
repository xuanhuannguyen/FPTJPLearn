using JPLearn.Core.Common.Entities;
using JPLearn.Core.ExamPractice;

namespace JPLearn.Core.ExamPractice.Entities;

public class ExamAttempt : BaseEntity
{
    public Guid UserId { get; set; }
    public string CourseCode { get; set; } = ExamCourseCodes.JPD113;
    public ExamCourse Course { get; set; } = null!;

    public Guid? BlueprintId { get; set; }
    public virtual ExamBlueprint? Blueprint { get; set; }

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SubmittedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int DurationMinutes { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectCount { get; set; }
    public double ScorePercent { get; set; }
    public string Mode { get; set; } = ExamAttemptModes.Practice;
    public string Status { get; set; } = ExamAttemptStatuses.InProgress;

    public ICollection<ExamAttemptAnswer> Answers { get; set; } = new List<ExamAttemptAnswer>();
}
