import { apiClient } from '../../../shared/api/axios';
import type { SpeakingCourse, SpeakingLesson, SpeakingLessonDetail } from '../types/speaking.types';

export const speakingApi = {
  getCourses: async (): Promise<SpeakingCourse[]> => {
    const response = await apiClient.get<{ courses: SpeakingCourse[] }>('/speaking/courses');
    return response.data.courses;
  },

  getLessonsByCourse: async (courseCode: string): Promise<SpeakingLesson[]> => {
    const response = await apiClient.get<{ lessons: SpeakingLesson[] }>(`/speaking/${courseCode}/lessons`);
    return response.data.lessons;
  },

  getLesson: async (lessonId: string): Promise<SpeakingLessonDetail> => {
    const response = await apiClient.get<SpeakingLessonDetail>(`/speaking/lessons/${lessonId}`);
    return response.data;
  },
};
