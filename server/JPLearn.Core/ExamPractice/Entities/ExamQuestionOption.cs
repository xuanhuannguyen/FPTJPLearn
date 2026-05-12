using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.ExamPractice.Entities;

public class ExamQuestionOption : BaseEntity
{
    public Guid QuestionId { get; set; }
    public ExamQuestion Question { get; set; } = null!;

    public string Label { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }
}
