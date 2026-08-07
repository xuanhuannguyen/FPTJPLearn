import { fetchStatic } from '../../../shared/services/staticDataService';
import type { SpeakingCourse, SpeakingLesson, SpeakingLessonDetail } from '../types/speaking.types';

export const speakingApi = {
  getCourses: async (): Promise<SpeakingCourse[]> => {
    return fetchStatic<SpeakingCourse[]>('speaking/courses.json');
  },

  getLessonsByCourse: async (courseCode: string): Promise<SpeakingLesson[]> => {
    return fetchStatic<SpeakingLesson[]>(`speaking/${courseCode}/lessons.json`);
  },

  getLesson: async (lessonId: string): Promise<SpeakingLessonDetail> => {
    const courseCode = lessonId.includes('1113') ? 'jpd113' : lessonId.includes('1123') ? 'jpd123' : 'jpd133';
    return fetchStatic<SpeakingLessonDetail>(`speaking/${courseCode}/lessons/${lessonId}.json`);
  },
};
