using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.ExamPractice.Entities;

public class ExamPracticeProgress : BaseEntity
{
    public Guid UserId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public int TotalCompleted { get; set; }
}
