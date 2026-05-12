import { apiClient } from '../../../shared/api/axios';
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
    const response = await apiClient.get<{ courses: VocabularyCourse[] }>('/vocabulary/courses');
    return response.data.courses;
  },

  getLessonsByCourse: async (courseCode: string): Promise<StaticVocabularyLesson[]> => {
    const response = await apiClient.get<{ lessons: StaticVocabularyLesson[] }>(`/vocabulary/${courseCode}/lessons`);
    return response.data.lessons;
  },

  getLessonById: async (lessonId: string): Promise<StaticVocabularyLessonDetail> => {
    const response = await apiClient.get<StaticVocabularyLessonDetail>(`/vocabulary/lessons/${lessonId}`);
    return response.data;
  },

  getPracticeCards: async (lessonId: string, mode: string = 'flashcard'): Promise<{ mode: string; cards: VocabularyPracticeCard[] }> => {
    const response = await apiClient.get<{ mode: string; cards: VocabularyPracticeCard[] }>(`/vocabulary/lessons/${lessonId}/practice`, { params: { mode } });
    return response.data;
  },

  getItemById: async (itemId: string): Promise<StaticVocabularyItem> => {
    const response = await apiClient.get<StaticVocabularyItem>(`/vocabulary/items/${itemId}`);
    return response.data;
  },

  search: async (query: string, courseCode?: string): Promise<StaticVocabularyItem[]> => {
    const response = await apiClient.get<{ items: StaticVocabularyItem[] }>('/vocabulary/search', { params: { query, courseCode } });
    return response.data.items;
  },

  recordView: async (itemId: string) => {
    const response = await apiClient.post(`/vocabulary/items/${itemId}/view`);
    return response.data;
  },

  recordFlashcardPractice: async (itemId: string) => {
    const response = await apiClient.post(`/vocabulary/items/${itemId}/flashcard-practice`);
    return response.data;
  },

  recordMultipleChoicePractice: async (itemId: string) => {
    const response = await apiClient.post(`/vocabulary/items/${itemId}/multiple-choice-practice`);
    return response.data;
  },

  recordTypingPractice: async (itemId: string) => {
    const response = await apiClient.post(`/vocabulary/items/${itemId}/typing-practice`);
    return response.data;
  },

  addToMemory: async (itemId: string) => {
    const response = await apiClient.post(`/vocabulary/items/${itemId}/memory`);
    return response.data;
  },

  getMemoryStatus: async (itemId: string): Promise<VocabularyMemoryStatus> => {
    const response = await apiClient.get<VocabularyMemoryStatus>(`/memory/vocabulary/from-item/${itemId}/status`);
    return response.data;
  }
};
