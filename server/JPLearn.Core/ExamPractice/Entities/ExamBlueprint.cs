using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.ExamPractice.Entities;

public class ExamBlueprint : BaseEntity
{
    public string CourseCode { get; set; } = string.Empty;
    public ExamCourse Course { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public int TimeLimitMinutes { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<ExamBlueprintRule> Rules { get; set; } = new List<ExamBlueprintRule>();
}
