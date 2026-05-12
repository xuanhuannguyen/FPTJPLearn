using JPLearn.Core.ExamPractice;

namespace JPLearn.Core.Admin.Exam;

public sealed record AdminExamQuestionDto(
    Guid Id,
    string CourseCode,
    string QuestionType,
    string Topic,
    string Level,
    string QuestionText,
    string Explanation,
    Guid? PassageId,
    AdminExamPassageDto? Passage,
    List<AdminExamQuestionOptionDto> Options,
    int OrderIndex,
    bool IsActive);

public sealed record AdminExamPassageDto(
    Guid Id,
    string CourseCode,
    string Title,
    string Content,
    string Level,
    string Topic,
    int OrderIndex,
    bool IsActive);

public sealed record AdminExamQuestionOptionDto(
    Guid Id,
    string Label,
    string Text,
    bool IsCorrect,
    int OrderIndex);

public sealed record AdminExamImportResult(
    int ImportedPassages,
    int CreatedQuestions,
    int UpdatedQuestions)
{
    public int TotalQuestions => CreatedQuestions + UpdatedQuestions;
}

public sealed class AdminExamQuestionInput
{
    public string CourseCode { get; set; } = ExamCourseCodes.JPD113;
    public string QuestionType { get; set; } = ExamQuestionTypes.Standalone;
    public string Topic { get; set; } = ExamQuestionTopics.Vocabulary;
    public string Level { get; set; } = "N5";
    public string QuestionText { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public Guid? PassageId { get; set; }
    public AdminExamPassageInput? Passage { get; set; }
    public List<AdminExamQuestionOptionInput> Options { get; set; } = [];
    public int? OrderIndex { get; set; }
    public bool? IsActive { get; set; }
}

public sealed class AdminExamPassageInput
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Level { get; set; }
    public string? Topic { get; set; }
    public int? OrderIndex { get; set; }
}

public sealed class AdminExamQuestionOptionInput
{
    public Guid? Id { get; set; }
    public string? Label { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}

public sealed class AdminExamImportFileInput
{
    public string CourseCode { get; set; } = ExamCourseCodes.JPD113;
    public string Level { get; set; } = "N5";
    public List<AdminExamImportPassageInput> Passages { get; set; } = [];
    public List<AdminExamImportQuestionInput> Questions { get; set; } = [];
}

public sealed class AdminExamImportPassageInput
{
    public string? LocalId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Level { get; set; }
    public string? Topic { get; set; }
    public int? OrderIndex { get; set; }
}

public sealed class AdminExamImportQuestionInput
{
    public string? LocalId { get; set; }
    public string QuestionType { get; set; } = ExamQuestionTypes.Standalone;
    public string Topic { get; set; } = ExamQuestionTopics.Vocabulary;
    public string? Level { get; set; }
    public string? PassageLocalId { get; set; }
    public AdminExamPassageInput? Passage { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
    public List<AdminExamQuestionOptionInput> Options { get; set; } = [];
}
