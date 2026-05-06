import { apiClient } from '../../../shared/api/axios';
import type {
  DueCardsResponse,
  ResetProgressPayload,
  ReviewAnswerResult,
  ReviewCard,
  ReviewSessionPayload,
} from '../types';

export const reviewApi = {
  getDueCards: async (listId: string) => {
    const response = await apiClient.get<DueCardsResponse>(`/review/${listId}/due`);
    return response.data;
  },

  getLearnedCards: async (listId: string, scope: 'mastered' | 'reviewed' | 'all') => {
    const response = await apiClient.get<{ cards: ReviewCard[] }>(`/review/${listId}/learned`, {
      params: { scope },
    });
    return response.data.cards;
  },

  getCardsByLevel: async (listId: string, minLevel: number, maxLevel: number) => {
    const response = await apiClient.get<{ cards: ReviewCard[] }>(`/review/${listId}/levels`, {
      params: { minLevel, maxLevel },
    });
    return response.data.cards;
  },

  getAllCards: async (listId: string) => {
    const response = await apiClient.get<{ cards: ReviewCard[] }>(`/review/${listId}/all`);
    return response.data.cards;
  },

  submitAnswer: async (payload: {
    itemId: string;
    quality: number;
    mode: string;
    sessionType: string;
  }) => {
    const response = await apiClient.post<ReviewAnswerResult>('/review/answer', payload);
    return response.data;
  },

  saveSession: async (payload: ReviewSessionPayload) => {
    const response = await apiClient.post<{ sessionId: string }>('/review/session', payload);
    return response.data;
  },

  resetProgress: async (listId: string, payload: ResetProgressPayload) => {
    const response = await apiClient.post<{ success: boolean; affectedCount: number }>(
      `/review/${listId}/reset`,
      payload
    );
    return response.data;
  },
};
