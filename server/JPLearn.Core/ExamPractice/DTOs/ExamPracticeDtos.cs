using JPLearn.Core.ExamPractice;

namespace JPLearn.Core.ExamPractice.DTOs;

public class ExamTopicDto
{
    public string Topic { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int QuestionCount { get; set; }
}

public class ExamCourseDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AccessTier { get; set; } = string.Empty;
    public string? PackageCode { get; set; }
    public bool IsLocked { get; set; }
    public int QuestionCount { get; set; }
    public int PassageCount { get; set; }
}

public class ExamPassageDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
}

public class ExamQuestionOptionDto
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

public class ExamQuestionReviewOptionDto : ExamQuestionOptionDto
{
    public bool IsCorrect { get; set; }
}

public class ExamQuestionDto
{
    public Guid Id { get; set; }
    public string QuestionType { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string QuestionText { get; set; } = string.Empty;
    public Guid? PassageId { get; set; }
}

public class ExamQuestionDetailDto : ExamQuestionDto
{
    public ExamPassageDto? Passage { get; set; }
    public List<ExamQuestionOptionDto> Options { get; set; } = [];
}

public class ExamAnswerQuestionDto
{
    public Guid SelectedOptionId { get; set; }
}

public class ExamAnswerResultDto
{
    public Guid QuestionId { get; set; }
    public Guid SelectedOptionId { get; set; }
    public Guid CorrectOptionId { get; set; }
    public bool IsCorrect { get; set; }
    public string Explanation { get; set; } = string.Empty;
}

public class StartExamAttemptDto
{
    public string CourseCode { get; set; } = ExamCourseCodes.JPD113;
    public string? Level { get; set; }
    public List<string> Topics { get; set; } = [];
    public int QuestionCount { get; set; } = ExamDefaults.AttemptQuestionCount;
    public int DurationMinutes { get; set; } = ExamDefaults.AttemptDurationMinutes;
    public string Mode { get; set; } = ExamAttemptModes.Practice;
}

public class SaveExamAttemptAnswerDto
{
    public Guid QuestionId { get; set; }
    public Guid SelectedOptionId { get; set; }
}

public class ExamAttemptAnswerDto
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? SelectedOptionId { get; set; }
    public bool? IsCorrect { get; set; }
    public DateTime? AnsweredAt { get; set; }
    public int SequenceNumber { get; set; }
}

public class ExamAttemptQuestionDto : ExamQuestionDetailDto
{
    public Guid AttemptAnswerId { get; set; }
    public Guid? SelectedOptionId { get; set; }
    public bool? IsCorrect { get; set; }
    public int SequenceNumber { get; set; }
}

public class ExamAttemptDto
{
    public Guid Id { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public int DurationMinutes { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectCount { get; set; }
    public double ScorePercent { get; set; }
    public List<ExamAttemptQuestionDto> Questions { get; set; } = [];
}

public class ExamAttemptReviewQuestionDto : ExamQuestionDto
{
    public ExamPassageDto? Passage { get; set; }
    public List<ExamQuestionReviewOptionDto> Options { get; set; } = [];
    public Guid? SelectedOptionId { get; set; }
    public Guid CorrectOptionId { get; set; }
    public bool IsCorrect { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public int SequenceNumber { get; set; }
}

public class ExamAttemptReviewDto
{
    public Guid Id { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectCount { get; set; }
    public double ScorePercent { get; set; }
    public List<ExamAttemptReviewQuestionDto> Questions { get; set; } = [];
}
