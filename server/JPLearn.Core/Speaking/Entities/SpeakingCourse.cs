using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Speaking.Entities;

public class SpeakingCourse : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AccessTier { get; set; } = SpeakingAccessTiers.Free;
    public string? PackageCode { get; set; }
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<SpeakingLesson> Lessons { get; set; } = new List<SpeakingLesson>();
}
