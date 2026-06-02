using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Speaking.Entities;

public class SpeakingLesson : BaseEntity
{
    public Guid CourseId { get; set; }
    public SpeakingCourse Course { get; set; } = null!;

    public string CourseCode { get; set; } = string.Empty;
    public int LessonNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AccessTier { get; set; } = SpeakingAccessTiers.Free;
    public string? PackageCode { get; set; }
    public string LessonType { get; set; } = SpeakingLessonTypes.Reading;
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<SpeakingSentence> Sentences { get; set; } = new List<SpeakingSentence>();
}
