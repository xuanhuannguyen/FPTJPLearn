using JPLearn.Core.Speaking.DTOs;

namespace JPLearn.Core.Speaking;

public interface ISpeakingService
{
    Task<List<SpeakingCourseDto>> GetCoursesAsync(Guid userId);
    Task<List<SpeakingLessonDto>?> GetLessonsByCourseAsync(Guid userId, string courseCode);
    Task<SpeakingLessonDetailDto?> GetLessonDetailAsync(Guid userId, Guid lessonId);
}
