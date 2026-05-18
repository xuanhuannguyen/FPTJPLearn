import { apiClient } from '../../../shared/api/axios';
import { fetchStatic } from '../../../shared/services/staticDataService';
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
    return fetchStatic<GrammarLevelSummary[]>('grammar/levels.json');
  },

  getLessonsByLevel: async (level: GrammarLevel, courseCode?: string): Promise<GrammarLesson[]> => {
    const lessons = await fetchStatic<GrammarLesson[]>(`grammar/${level}/lessons.json`);
    return courseCode ? lessons.filter((lesson) => lesson.courseCode === courseCode) : lessons;
  },

  getLessonById: async (lessonId: string): Promise<{ lesson: GrammarLesson; patterns: GrammarPattern[] }> => {
    const level = await findLessonLevel(lessonId);
    return fetchStatic<{ lesson: GrammarLesson; patterns: GrammarPattern[] }>(`grammar/${level}/lessons/${lessonId}.json`);
  },

  getPatternById: async (patternId: string): Promise<GrammarPattern> => {
    const patterns = await fetchStatic<GrammarPattern[]>('grammar/patterns.json');
    const pattern = patterns.find((candidate) => candidate.id === patternId);
    if (!pattern) throw new Error(`Grammar pattern not found: ${patternId}`);
    return pattern;
  },

  searchPatterns: async (query: string): Promise<GrammarPattern[]> => {
    const normalizedQuery = query.trim().toLowerCase();
    if (!normalizedQuery) return [];
    const patterns = await fetchStatic<GrammarPattern[]>('grammar/patterns.json');
    return patterns.filter((pattern) =>
      [pattern.pattern, pattern.title, pattern.meaning, pattern.structure, pattern.notes]
        .some((value) => value?.toLowerCase().includes(normalizedQuery))
    );
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
    return fetchStatic<GrammarExercise[]>(`grammar/exercises/${patternId}.json`);
  },

  checkExercise: async (
    exerciseId: string,
    payload: { answerText?: string; selectedOptionOrder?: string[] }
  ): Promise<GrammarExerciseCheckResult> => {
    const exercise = await findExercise(exerciseId);
    const expectedAnswers = [
      exercise.expectedAnswer,
      ...parseJsonArray((exercise as GrammarExerciseAnswer).acceptableAnswersJson),
      ...((exercise as GrammarExerciseAnswer).acceptableAnswers || []),
    ].filter(Boolean);
    const answer = exercise.exerciseType === 'arrange'
      ? (payload.selectedOptionOrder || []).join('')
      : payload.answerText || '';
    const isCorrect = expectedAnswers.some((expected) => normalizeAnswer(expected) === normalizeAnswer(answer));

    return {
      exerciseId,
      isCorrect,
      score: isCorrect ? 100 : 0,
      feedback: isCorrect ? 'Chính xác.' : 'Chưa đúng, hãy xem lại đáp án mẫu.',
      expectedAnswer: exercise.expectedAnswer || '',
      correctOrderJson: (exercise as GrammarExerciseAnswer).correctOrderJson,
      starAnswer: (exercise as GrammarExerciseAnswer).starAnswer,
      attemptId: `static-${exerciseId}`,
    };
  },

  revealExerciseAnswer: async (exerciseId: string): Promise<GrammarExerciseAnswer> => {
    const exercise = await findExercise(exerciseId);
    return {
      ...exercise,
      expectedAnswer: exercise.expectedAnswer || '',
      acceptableAnswers: [
        ...parseJsonArray((exercise as GrammarExerciseAnswer).acceptableAnswersJson),
        ...((exercise as GrammarExerciseAnswer).acceptableAnswers || []),
      ],
      correctOrder: [
        ...parseJsonArray((exercise as GrammarExerciseAnswer).correctOrderJson),
        ...((exercise as GrammarExerciseAnswer).correctOrder || []),
      ],
    };
  },
};

async function findLessonLevel(lessonId: string): Promise<GrammarLevel> {
  const levels = await fetchStatic<GrammarLevelSummary[]>('grammar/levels.json');
  for (const level of [...new Set(levels.map((item) => item.level))]) {
    const lessons = await fetchStatic<GrammarLesson[]>(`grammar/${level}/lessons.json`);
    if (lessons.some((lesson) => lesson.id === lessonId)) return level;
  }
  throw new Error(`Grammar lesson not found: ${lessonId}`);
}

async function findExercise(exerciseId: string): Promise<GrammarExercise> {
  const patterns = await fetchStatic<GrammarPattern[]>('grammar/patterns.json');
  for (const pattern of patterns) {
    const exercises = await fetchStatic<GrammarExercise[]>(`grammar/exercises/${pattern.id}.json`);
    const exercise = exercises.find((candidate) => candidate.id === exerciseId);
    if (exercise) return exercise;
  }
  throw new Error(`Grammar exercise not found: ${exerciseId}`);
}

function parseJsonArray(value?: string): string[] {
  if (!value) return [];
  try {
    const parsed = JSON.parse(value);
    return Array.isArray(parsed) ? parsed.map(String) : [];
  } catch {
    return [];
  }
}

function normalizeAnswer(value: string): string {
  return value
    .trim()
    .toLowerCase()
    .replace(/\s+/g, '')
    .replace(/[。！？!?.,，、]/g, '');
}
