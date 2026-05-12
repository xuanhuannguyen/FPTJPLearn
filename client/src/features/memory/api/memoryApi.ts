import { apiClient } from '../../../shared/api/axios';
import type {
  AddGrammarToMemoryResult,
  MemoryAnswerResult,
  MemoryCardsResponse,
  MemoryGrammarStatus,
  MemoryItemType,
  MemoryScope,
  MemorySummary,
  MemoryTypeSummary,
  ResetMemoryResult,
} from '../types/memory.types';

const memoryPathByType: Record<MemoryItemType, string> = {
  grammar: 'grammar',
  kanji: 'kanji',
  vocabulary: 'vocabulary',
};

export const memoryApi = {
  getSummary: async (): Promise<MemorySummary> => {
    const response = await apiClient.get('/memory/summary');
    return response.data;
  },

  getTypeSummary: async (type: MemoryItemType): Promise<MemoryTypeSummary> => {
    const response = await apiClient.get(`/memory/${memoryPathByType[type]}/summary`);
    return response.data;
  },

  getGrammarSummary: async (): Promise<MemoryTypeSummary> => {
    return memoryApi.getTypeSummary('grammar');
  },

  addGrammarFromPattern: async (patternId: string): Promise<AddGrammarToMemoryResult> => {
    const response = await apiClient.post(`/memory/grammar/from-pattern/${patternId}`);
    return response.data;
  },

  getGrammarPatternStatus: async (patternId: string): Promise<MemoryGrammarStatus> => {
    const response = await apiClient.get(`/memory/grammar/from-pattern/${patternId}/status`);
    return response.data;
  },

  addKanjiFromItem: async (kanjiId: string): Promise<any> => {
    const response = await apiClient.post(`/memory/kanji/from-item/${kanjiId}`);
    return response.data;
  },

  getKanjiItemStatus: async (kanjiId: string): Promise<MemoryGrammarStatus> => {
    const response = await apiClient.get(`/memory/kanji/from-item/${kanjiId}/status`);
    return response.data;
  },

  removeKanjiItem: async (memoryItemId: string): Promise<{ success: boolean }> => {
    const response = await apiClient.delete(`/memory/kanji/${memoryItemId}`);
    return response.data;
  },

  removeGrammarItem: async (memoryItemId: string): Promise<{ success: boolean }> => {
    const response = await apiClient.delete(`/memory/grammar/${memoryItemId}`);
    return response.data;
  },

<<<<<<< HEAD
  removeVocabularyItem: async (memoryItemId: string): Promise<{ success: boolean }> => {
    const response = await apiClient.delete(`/memory/vocabulary/${memoryItemId}`);
    return response.data;
  },

  removeItem: async (type: MemoryItemType, memoryItemId: string): Promise<{ success: boolean }> => {
    const response = await apiClient.delete(`/memory/${memoryPathByType[type]}/${memoryItemId}`);
    return response.data;
  },

=======
>>>>>>> 86b7c57576a19ea16bf7bfdd03579c7aef23e5bf
  getCards: async (type: MemoryItemType, scope: MemoryScope = 'due'): Promise<MemoryCardsResponse> => {
    const response = await apiClient.get(`/memory/${memoryPathByType[type]}/cards`, { params: { scope } });
    return response.data;
  },

  getGrammarCards: async (scope: MemoryScope = 'due'): Promise<MemoryCardsResponse> => {
    return memoryApi.getCards('grammar', scope);
  },

  submitAnswer: async (type: MemoryItemType, memoryItemId: string, quality: number): Promise<MemoryAnswerResult> => {
    const response = await apiClient.post(`/memory/${memoryPathByType[type]}/answer`, { memoryItemId, quality });
    return response.data;
  },

  submitGrammarAnswer: async (memoryItemId: string, quality: number): Promise<MemoryAnswerResult> => {
    return memoryApi.submitAnswer('grammar', memoryItemId, quality);
  },

  resetProgress: async (type: MemoryItemType): Promise<ResetMemoryResult> => {
    const response = await apiClient.post(`/memory/${memoryPathByType[type]}/reset`, { scope: 'all', memoryItemIds: [] });
    return response.data;
  },

  resetGrammarProgress: async (): Promise<ResetMemoryResult> => {
    return memoryApi.resetProgress('grammar');
  },
};
