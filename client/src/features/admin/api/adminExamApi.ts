import { apiClient } from '../../../shared/api/axios';
import type { AdminExamImportResult, AdminExamQuestion, AdminExamQuestionPayload } from '../types/adminExam.types';

export const adminExamApi = {
  getQuestions: async (params?: { courseCode?: string; topic?: string; includeInactive?: boolean }): Promise<AdminExamQuestion[]> => {
    const response = await apiClient.get<{ questions: AdminExamQuestion[] }>('/admin/exam/questions', { params });
    return response.data.questions;
  },

  createQuestion: async (payload: AdminExamQuestionPayload): Promise<AdminExamQuestion> => {
    const response = await apiClient.post<AdminExamQuestion>('/admin/exam/questions', payload);
    return response.data;
  },

  updateQuestion: async (questionId: string, payload: AdminExamQuestionPayload): Promise<AdminExamQuestion> => {
    const response = await apiClient.put<AdminExamQuestion>(`/admin/exam/questions/${questionId}`, payload);
    return response.data;
  },

  deleteQuestion: async (questionId: string): Promise<void> => {
    await apiClient.delete(`/admin/exam/questions/${questionId}`);
  },

  getImportTemplate: async (type: 'standard' | 'reading' = 'standard'): Promise<unknown> => {
    const response = await apiClient.get('/admin/exam/questions/import-template', { params: { type } });
    return response.data;
  },

  importJson: async (payload: unknown): Promise<AdminExamImportResult> => {
    const response = await apiClient.post<AdminExamImportResult>('/admin/exam/questions/import-json', payload);
    return response.data;
  },
};
