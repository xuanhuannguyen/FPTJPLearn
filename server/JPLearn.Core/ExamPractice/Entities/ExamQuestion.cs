using JPLearn.Core.Common.Entities;
using JPLearn.Core.ExamPractice;

namespace JPLearn.Core.ExamPractice.Entities;

public class ExamQuestion : BaseEntity
{
    public string CourseCode { get; set; } = ExamCourseCodes.JPD113;
    public virtual ExamCourse? Course { get; set; }

    public string QuestionType { get; set; } = ExamQuestionTypes.Standalone;
    public string Topic { get; set; } = string.Empty;
    public virtual ExamTopic? TopicDefinition { get; set; }

    public string? Level { get; set; }

    public Guid? PassageId { get; set; }
    public virtual ExamPassage? Passage { get; set; }

    public string QuestionText { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<ExamQuestionOption> Options { get; set; } = new List<ExamQuestionOption>();
    public virtual ICollection<ExamAttemptAnswer> AttemptAnswers { get; set; } = new List<ExamAttemptAnswer>();
}
