using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.ExamPractice.Entities;

public class ExamAttemptAnswer : BaseEntity
{
    public Guid AttemptId { get; set; }
    public ExamAttempt Attempt { get; set; } = null!;

    public Guid QuestionId { get; set; }
    public ExamQuestion Question { get; set; } = null!;

    public Guid? SelectedOptionId { get; set; }
    public ExamQuestionOption? SelectedOption { get; set; }

    public bool? IsCorrect { get; set; }
    public DateTime? AnsweredAt { get; set; }
    public int SequenceNumber { get; set; }
}
