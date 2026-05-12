using JPLearn.Core.Common.Entities;
using JPLearn.Core.ExamPractice;

namespace JPLearn.Core.ExamPractice.Entities;

public class ExamPassage : BaseEntity
{
    public string CourseCode { get; set; } = ExamCourseCodes.JPD113;
    public virtual ExamCourse? Course { get; set; }

    public string Topic { get; set; } = string.Empty;
    public virtual ExamTopic? TopicDefinition { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Level { get; set; } = "N5";
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<ExamQuestion>? Questions { get; set; } = new List<ExamQuestion>();
}
