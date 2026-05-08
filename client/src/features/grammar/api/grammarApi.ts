import { apiClient } from '../../../shared/api/axios';
import type {
  AddGrammarStudyResult,
  GrammarAnswerResult,
  GrammarDueResponse,
  GrammarExercise,
  GrammarExerciseAnswer,
  GrammarExerciseCheckResult,
  GrammarLevel,
  GrammarLevelSummary,
  GrammarLesson,
  GrammarPattern,
  GrammarProgressSummary,
} from '../types/grammar.types';

export const grammarApi = {
  getLevels: async (): Promise<GrammarLevelSummary[]> => {
    const response = await apiClient.get('/grammar/levels');
    return response.data;
  },

  getLessonsByLevel: async (level: GrammarLevel): Promise<GrammarLesson[]> => {
    const response = await apiClient.get(`/grammar/${level}/lessons`);
    return response.data.lessons;
  },

  getLessonById: async (lessonId: string): Promise<{ lesson: GrammarLesson; patterns: GrammarPattern[] }> => {
    const response = await apiClient.get(`/grammar/lessons/${lessonId}`);
    return response.data;
  },

  getPatternById: async (patternId: string): Promise<GrammarPattern> => {
    const response = await apiClient.get(`/grammar/patterns/${patternId}`);
    return response.data;
  },

  searchPatterns: async (query: string): Promise<GrammarPattern[]> => {
    const response = await apiClient.get('/grammar/search', { params: { query } });
    return response.data.patterns;
  },

  addToStudy: async (patternId: string): Promise<AddGrammarStudyResult> => {
    const response = await apiClient.post(`/grammar/patterns/${patternId}/study`);
    return response.data;
  },

  removeFromStudy: async (patternId: string): Promise<{ success: boolean }> => {
    const response = await apiClient.delete(`/grammar/patterns/${patternId}/study`);
    return response.data;
  },

  getProgressSummary: async (): Promise<GrammarProgressSummary> => {
    const response = await apiClient.get('/grammar/progress');
    return response.data;
  },

  getDueCards: async (): Promise<GrammarDueResponse> => {
    const response = await apiClient.get('/grammar/review/due');
    return response.data;
  },

  submitReviewAnswer: async (patternId: string, quality: number): Promise<GrammarAnswerResult> => {
    const response = await apiClient.post('/grammar/review/answer', { patternId, quality });
    return response.data;
  },

  getPatternExercises: async (patternId: string): Promise<GrammarExercise[]> => {
    const response = await apiClient.get(`/grammar/patterns/${patternId}/exercises`);
    return response.data.exercises;
  },

  checkExercise: async (
    exerciseId: string,
    payload: { answerText?: string; selectedOptionOrder?: string[] }
  ): Promise<GrammarExerciseCheckResult> => {
    const response = await apiClient.post(`/grammar/exercises/${exerciseId}/check`, payload);
    return response.data;
  },

  revealExerciseAnswer: async (exerciseId: string): Promise<GrammarExerciseAnswer> => {
    const response = await apiClient.get(`/grammar/exercises/${exerciseId}/answer`);
    return response.data;
  },
};
