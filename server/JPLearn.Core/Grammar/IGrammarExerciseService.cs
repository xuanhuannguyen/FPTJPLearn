using JPLearn.Core.Grammar.DTOs;

namespace JPLearn.Core.Grammar;

public interface IGrammarExerciseService
{
    Task<List<GrammarExerciseDto>?> GetExercisesByPatternAsync(Guid patternId);
    Task<GrammarExerciseCheckResultDto?> CheckAnswerAsync(Guid userId, Guid exerciseId, CheckGrammarExerciseDto dto);
    Task<GrammarExerciseAnswerDto?> RevealAnswerAsync(Guid exerciseId);
    Task<AiGrammarEvaluationResultDto?> EvaluateWithAiAsync(Guid userId, Guid exerciseId, AiEvaluateGrammarExerciseDto dto);
}
