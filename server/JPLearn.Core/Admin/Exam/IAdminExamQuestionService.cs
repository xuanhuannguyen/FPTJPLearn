namespace JPLearn.Core.Admin.Exam;

public interface IAdminExamQuestionService
{
    Task<List<AdminExamQuestionDto>> GetQuestionsAsync(string? courseCode = null, string? topic = null, bool includeInactive = false);
    Task<AdminExamQuestionDto?> GetQuestionAsync(Guid questionId);
    AdminExamImportFileInput GetImportTemplate(string type);
    Task<AdminExamImportResult> ImportQuestionsAsync(AdminExamImportFileInput input);
    Task<AdminExamQuestionDto> CreateQuestionAsync(AdminExamQuestionInput input);
    Task<AdminExamQuestionDto?> UpdateQuestionAsync(Guid questionId, AdminExamQuestionInput input);
    Task<bool> DeleteQuestionAsync(Guid questionId);
}
