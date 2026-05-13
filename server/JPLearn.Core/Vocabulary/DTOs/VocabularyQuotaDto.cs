using System;

namespace JPLearn.Core.Vocabulary.DTOs;

public class VocabularyQuotaDto
{
    public bool IsPremium { get; set; }
    public int UsedCount { get; set; }
    public int MaxCount { get; set; }
    public int RemainingCount { get; set; }
    public string Period { get; set; } = "total"; // "total" or "daily"
}
