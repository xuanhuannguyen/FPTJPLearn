import { apiClient } from '../../../shared/api/axios';

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

export const vocabularyApi = {
  importJSON: async (data: ImportVocabularyDto) => {
    const response = await apiClient.post('/vocabulary/lists/import', data);
    return response.data;
  },
  
  getLists: async () => {
    const response = await apiClient.get('/vocabulary/lists');
    return response.data;
  },

  getListById: async (id: string) => {
    const response = await apiClient.get(`/vocabulary/lists/${id}`);
    return response.data;
  },

  deleteList: async (id: string) => {
    const response = await apiClient.delete(`/vocabulary/lists/${id}`);
    return response.data;
  },

  deleteItem: async (itemId: string) => {
    const response = await apiClient.delete(`/vocabulary/items/${itemId}`);
    return response.data;
  },

  addItem: async (listId: string, wordData: any) => {
    const response = await apiClient.post(`/vocabulary/lists/${listId}/items`, wordData);
    return response.data;
  }
};
