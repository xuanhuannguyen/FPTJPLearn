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
    return fetchStatic<StaticVocabularyLesson[]>(`vocabulary/${courseCode}/lessons.json`);
  },

  getLessonById: async (lessonId: string): Promise<StaticVocabularyLessonDetail> => {
    const courseCode = lessonId.includes('1113') ? 'jpd113' : 'jpd123';
    return fetchStatic<StaticVocabularyLessonDetail>(`vocabulary/${courseCode}/lessons/${lessonId}.json`);
  },

  getPracticeCards: async (lessonId: string, mode: string = 'flashcard'): Promise<{ mode: string; cards: VocabularyPracticeCard[] }> => {
    const detail = await staticVocabularyApi.getLessonById(lessonId);
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

function buildOptions(items: StaticVocabularyItem[], correctAnswer: string, index: number): string[] {
  const distractors = items
    .filter((item) => item.meaning !== correctAnswer)
    .slice(index + 1)
    .concat(items.slice(0, index))
    .map((item) => item.meaning)
    .slice(0, 3);

  return [correctAnswer, ...distractors].sort((a, b) => a.localeCompare(b));
}
