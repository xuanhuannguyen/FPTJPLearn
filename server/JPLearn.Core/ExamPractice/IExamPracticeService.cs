using JPLearn.Core.ExamPractice.DTOs;

namespace JPLearn.Core.ExamPractice;

public interface IExamPracticeService
{
    Task<List<ExamCourseDto>> GetCoursesAsync(Guid userId);
    Task<List<ExamTopicDto>> GetTopicsAsync(Guid userId, string? courseCode = null, string? level = null);
    Task<List<ExamQuestionDto>> GetQuestionsAsync(Guid userId, string? courseCode = null, string? topic = null, string? level = null);
    Task<ExamQuestionDetailDto?> GetQuestionAsync(Guid userId, Guid questionId);
    Task<ExamAnswerResultDto?> AnswerQuestionAsync(Guid userId, Guid questionId, ExamAnswerQuestionDto dto);
    Task<ExamAttemptDto?> StartAttemptAsync(Guid userId, StartExamAttemptDto dto);
    Task<ExamAttemptDto?> GetAttemptAsync(Guid userId, Guid attemptId);
    Task<ExamAttemptAnswerDto?> SaveAttemptAnswerAsync(Guid userId, Guid attemptId, SaveExamAttemptAnswerDto dto);
    Task<ExamAttemptDto?> SubmitAttemptAsync(Guid userId, Guid attemptId);
    Task<ExamAttemptReviewDto?> GetAttemptReviewAsync(Guid userId, Guid attemptId);
}
