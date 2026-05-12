using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.ExamPractice.Entities;

public class ExamBlueprintRule : BaseEntity
{
    public Guid BlueprintId { get; set; }
    public ExamBlueprint Blueprint { get; set; } = null!;

    public string Topic { get; set; } = string.Empty;
    public int QuestionCount { get; set; }
}
