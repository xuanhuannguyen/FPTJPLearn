using JPLearn.Core.Common.Entities;

namespace JPLearn.Core.Settings.Entities;

public class AppSetting : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
}
