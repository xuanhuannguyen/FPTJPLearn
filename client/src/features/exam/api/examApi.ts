import { apiClient } from '../../../shared/api/axios';
import type {
  ExamAnswerResult,
  ExamAttempt,
  ExamAttemptAnswer,
  ExamAttemptReview,
  ExamCourse,
  ExamQuestion,
  ExamQuestionDetail,
  ExamTopic,
  StartExamAttemptRequest,
} from '../types/exam.types';

export const examApi = {
  getCourses: async (): Promise<ExamCourse[]> => {
    const response = await apiClient.get<{ courses: ExamCourse[] }>('/exam/courses');
    return response.data.courses;
  },

  getTopics: async (courseCode?: string, level?: string): Promise<ExamTopic[]> => {
    const response = await apiClient.get<{ topics: ExamTopic[] }>('/exam/topics', { params: { courseCode, level } });
    return response.data.topics;
  },

  getQuestions: async (params?: { courseCode?: string; topic?: string; level?: string }): Promise<ExamQuestion[]> => {
    const response = await apiClient.get<{ questions: ExamQuestion[] }>('/exam/questions', { params });
    return response.data.questions;
  },

  getQuestion: async (questionId: string): Promise<ExamQuestionDetail> => {
    const response = await apiClient.get<ExamQuestionDetail>(`/exam/questions/${questionId}`);
    return response.data;
  },

  answerQuestion: async (questionId: string, selectedOptionId: string): Promise<ExamAnswerResult> => {
    const response = await apiClient.post<ExamAnswerResult>(`/exam/questions/${questionId}/answer`, { selectedOptionId });
    return response.data;
  },

  startAttempt: async (payload: StartExamAttemptRequest = {}): Promise<ExamAttempt> => {
    const response = await apiClient.post<ExamAttempt>('/exam/attempts/start', payload);
    return response.data;
  },

  getAttempt: async (attemptId: string): Promise<ExamAttempt> => {
    const response = await apiClient.get<ExamAttempt>(`/exam/attempts/${attemptId}`);
    return response.data;
  },

  saveAttemptAnswer: async (attemptId: string, questionId: string, selectedOptionId: string): Promise<ExamAttemptAnswer> => {
    const response = await apiClient.post<ExamAttemptAnswer>(`/exam/attempts/${attemptId}/answers`, { questionId, selectedOptionId });
    return response.data;
  },

  submitAttempt: async (attemptId: string): Promise<ExamAttempt> => {
    const response = await apiClient.post<ExamAttempt>(`/exam/attempts/${attemptId}/submit`);
    return response.data;
  },

  getAttemptReview: async (attemptId: string): Promise<ExamAttemptReview> => {
    const response = await apiClient.get<ExamAttemptReview>(`/exam/attempts/${attemptId}/review`);
    return response.data;
  },
};
