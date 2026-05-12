using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.ExamPractice.Entities;

public class ExamCourse : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AccessTier { get; set; } = "free";
    public string? PackageCode { get; set; }
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<ExamPassage> Passages { get; set; } = new List<ExamPassage>();
    public ICollection<ExamQuestion> Questions { get; set; } = new List<ExamQuestion>();
    public ICollection<ExamAttempt> Attempts { get; set; } = new List<ExamAttempt>();
}
