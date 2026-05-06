using System.ComponentModel.DataAnnotations;

namespace JPLearn.Core.Vocabulary.DTOs;

public class ImportVocabularyDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public List<VocabularyWordDto> Words { get; set; } = new();
}

public class VocabularyWordDto
{
    [Required]
    public string Word { get; set; } = string.Empty;        // 行きます

    [Required]
    public string Reading { get; set; } = string.Empty;      // いきます

    [Required]
    public string Type { get; set; } = string.Empty;         // Động từ nhóm 1

    [Required]
    public string Meaning { get; set; } = string.Empty;      // Đi

    public string? Example { get; set; }                     // 学校に行きます。
    public string? ExampleMeaning { get; set; }              // Tôi đi đến trường.
}
