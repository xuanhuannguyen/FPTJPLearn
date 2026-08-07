import { apiClient } from '../../../shared/api/axios';
import { fetchStatic } from '../../../shared/services/staticDataService';
import type { 
  VocabularyCourse, 
  StaticVocabularyLesson, 
  StaticVocabularyLessonDetail, 
  StaticVocabularyItem, 
  VocabularyMemoryStatus,
  VocabularyPracticeCard 
} from '../types/vocabulary.types';

export const staticVocabularyApi = {
  getCourses: async (): Promise<VocabularyCourse[]> => {
    return fetchStatic<VocabularyCourse[]>('vocabulary/courses.json');
  },

  getLessonsByCourse: async (courseCode: string): Promise<StaticVocabularyLesson[]> => {
    const lessons = await fetchStatic<StaticVocabularyLesson[]>(`vocabulary/${courseCode}/lessons.json`);
    return lessons.map(normalizeVocabularyLessonAccess);
  },

  getLessonById: async (lessonId: string, requestedCourseCode?: string): Promise<StaticVocabularyLessonDetail> => {
    const courseCode = requestedCourseCode?.trim().toLowerCase() || courseCodeFromLessonId(lessonId);
    const detail = await fetchStatic<StaticVocabularyLessonDetail>(`vocabulary/${courseCode}/lessons/${lessonId}.json`);
    return {
      lesson: normalizeVocabularyLessonAccess(detail.lesson),
      items: detail.items.map(normalizeVocabularyItemAccess),
    };
  },

  getPracticeCards: async (lessonId: string, mode: string = 'flashcard', courseCode?: string): Promise<{ mode: string; cards: VocabularyPracticeCard[] }> => {
    const detail = await staticVocabularyApi.getLessonById(lessonId, courseCode);
    const cards = detail.items.map((item, index) => ({
      itemId: item.id,
      mode,
      prompt: mode === 'typing' ? item.meaning : item.word,
      promptReading: item.reading,
      correctAnswer: mode === 'typing' ? item.word : item.meaning,
      options: buildOptions(detail.items, item.meaning, index),
      word: item.word,
      reading: item.reading,
      meaning: item.meaning,
      exampleJapanese: item.exampleJapanese,
      exampleMeaning: item.exampleMeaning,
    }));
    return { mode, cards };
  },

  getItemById: async (itemId: string): Promise<StaticVocabularyItem> => {
    const items = await fetchStatic<StaticVocabularyItem[]>('vocabulary/items.json');
    const item = items.find((candidate) => candidate.id === itemId);
    if (!item) throw new Error(`Vocabulary item not found: ${itemId}`);
    return item;
  },

  search: async (query: string, courseCode?: string): Promise<StaticVocabularyItem[]> => {
    const normalizedQuery = query.trim().toLowerCase();
    if (!normalizedQuery) return [];
    const items = await fetchStatic<StaticVocabularyItem[]>('vocabulary/items.json');
    return items
      .map(normalizeVocabularyItemAccess)
      .filter((item) => !courseCode || item.courseCode === courseCode)
      .filter((item) =>
        [item.word, item.reading, item.meaning, item.wordType]
          .some((value) => value?.toLowerCase().includes(normalizedQuery))
      );
  },

  recordView: async (itemId: string) => {
    void itemId;
    return { success: true };
  },

  recordFlashcardPractice: async (itemId: string) => {
    void itemId;
    return { success: true };
  },

  recordMultipleChoicePractice: async (itemId: string) => {
    void itemId;
    return { success: true };
  },

  recordTypingPractice: async (itemId: string) => {
    void itemId;
    return { success: true };
  },

  addToMemory: async (itemId: string) => {
    const response = await apiClient.post(`/memory/vocabulary/from-item/${itemId}`);
    return response.data;
  },

  getMemoryStatus: async (itemId: string): Promise<VocabularyMemoryStatus> => {
    const response = await apiClient.get<VocabularyMemoryStatus>(`/memory/vocabulary/from-item/${itemId}/status`);
    return response.data;
  }
};

function normalizeVocabularyLessonAccess(lesson: StaticVocabularyLesson): StaticVocabularyLesson {
  return {
    ...lesson,
    packageCode: normalizeVocabularyPackageCode(lesson.packageCode, lesson.courseCode),
  };
}

function normalizeVocabularyItemAccess(item: StaticVocabularyItem): StaticVocabularyItem {
  return {
    ...item,
    packageCode: normalizeVocabularyPackageCode(item.packageCode, item.courseCode),
  };
}

function normalizeVocabularyPackageCode(packageCode: string | undefined, courseCode: string): string {
  const code = (packageCode || courseCode).trim().toLowerCase();
  return code.startsWith('vocab_') ? code : `vocab_${code}`;
}

function courseCodeFromLessonId(lessonId: string): string {
  if (lessonId.includes('1113')) return 'jpd113';
  if (lessonId.includes('1123')) return 'jpd123';
  if (lessonId.includes('1133')) return 'jpd133';
  throw new Error(`Unknown vocabulary lesson ID segment: ${lessonId}`);
}

function buildOptions(items: StaticVocabularyItem[], correctAnswer: string, index: number): string[] {
  const distractors = items
    .filter((item) => item.meaning !== correctAnswer)
    .slice(index + 1)
    .concat(items.slice(0, index))
    .map((item) => item.meaning)
    .slice(0, 3);

  return [correctAnswer, ...distractors].sort((a, b) => a.localeCompare(b));
}
