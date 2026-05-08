import { apiClient } from '../../../shared/api/axios';
import type {
  AddGrammarToMemoryResult,
  MemoryAnswerResult,
  MemoryCardsResponse,
  MemoryGrammarStatus,
  MemoryScope,
  MemorySummary,
  MemoryTypeSummary,
} from '../types/memory.types';

export const memoryApi = {
  getSummary: async (): Promise<MemorySummary> => {
    const response = await apiClient.get('/memory/summary');
    return response.data;
  },

  getGrammarSummary: async (): Promise<MemoryTypeSummary> => {
    const response = await apiClient.get('/memory/grammar/summary');
    return response.data;
  },

  addGrammarFromPattern: async (patternId: string): Promise<AddGrammarToMemoryResult> => {
    const response = await apiClient.post(`/memory/grammar/from-pattern/${patternId}`);
    return response.data;
  },

  getGrammarPatternStatus: async (patternId: string): Promise<MemoryGrammarStatus> => {
    const response = await apiClient.get(`/memory/grammar/from-pattern/${patternId}/status`);
    return response.data;
  },

  removeGrammarItem: async (memoryItemId: string): Promise<{ success: boolean }> => {
    const response = await apiClient.delete(`/memory/grammar/${memoryItemId}`);
    return response.data;
  },

  getGrammarCards: async (scope: MemoryScope = 'due'): Promise<MemoryCardsResponse> => {
    const response = await apiClient.get('/memory/grammar/cards', { params: { scope } });
    return response.data;
  },

  submitGrammarAnswer: async (memoryItemId: string, quality: number): Promise<MemoryAnswerResult> => {
    const response = await apiClient.post('/memory/grammar/answer', { memoryItemId, quality });
    return response.data;
  },
};
