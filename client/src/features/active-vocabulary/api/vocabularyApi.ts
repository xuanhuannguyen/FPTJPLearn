import { apiClient } from '../../../shared/api/axios';

export interface VocabularyList {
  id: string;
  name: string;
  description: string;
  wordCount: number;
  masteredCount: number;
  dueCount: number;
  createdAt: string;
}

export interface VocabularyItem {
  id: string;
  word: string;
  reading: string;
  wordType: string;
  meaning: string;
  exampleSentence?: string;
  exampleMeaning?: string;
  orderIndex: number;
  level: number;
  status: string;
}

export interface VocabularyListDetail extends VocabularyList {
  items: VocabularyItem[];
}

export interface ImportVocabularyDto {
  name: string;
  description?: string;
  words: {
    word: string;
    reading?: string;
    type?: string;
    meaning: string;
    example?: string;
    exampleMeaning?: string;
  }[];
}

export interface AddVocabularyItemDto {
  word: string;
  reading: string;
  type: string;
  meaning: string;
  example: string;
  exampleMeaning: string;
}

interface AddVocabularyItemResponse {
  itemId: string;
}

export const vocabularyApi = {
  importJSON: async (data: ImportVocabularyDto) => {
    const response = await apiClient.post('/active-vocabulary/lists/import', data);
    return response.data;
  },
  
  getLists: async () => {
    const response = await apiClient.get<VocabularyList[]>('/active-vocabulary/lists');
    return response.data;
  },

  getListById: async (id: string) => {
    const response = await apiClient.get<VocabularyListDetail>(`/active-vocabulary/lists/${id}`);
    return response.data;
  },

  deleteList: async (id: string) => {
    const response = await apiClient.delete(`/active-vocabulary/lists/${id}`);
    return response.data;
  },

  deleteItem: async (itemId: string) => {
    const response = await apiClient.delete(`/active-vocabulary/items/${itemId}`);
    return response.data;
  },

  addItem: async (listId: string, wordData: AddVocabularyItemDto) => {
    const response = await apiClient.post<AddVocabularyItemResponse>(`/active-vocabulary/lists/${listId}/items`, wordData);
    return response.data;
  }
};
