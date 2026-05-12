using JPLearn.Core.Common.Entities;
using JPLearn.Core.ExamPractice;

namespace JPLearn.Core.ExamPractice.Entities;

public class ExamTopic : BaseEntity
{
    public string CourseCode { get; set; } = ExamCourseCodes.JPD113;
    public virtual ExamCourse? Course { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<ExamPassage>? Passages { get; set; } = new List<ExamPassage>();
    public virtual ICollection<ExamQuestion>? Questions { get; set; } = new List<ExamQuestion>();
}
