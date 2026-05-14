export interface SpeakingCourse {
  id: string;
  code: string;
  title: string;
  description?: string | null;
  accessTier: string;
  packageCode?: string | null;
  isLocked: boolean;
  lessonCount: number;
  sentenceCount: number;
}

export interface SpeakingLesson {
  id: string;
  courseCode: string;
  lessonNumber: number;
  title: string;
  description?: string | null;
  accessTier: string;
  packageCode?: string | null;
  isLocked: boolean;
  sentenceCount: number;
}

export interface SpeakingSentence {
  id: string;
  lessonId: string;
  sentenceNumber: number;
  plainText: string;
  romaji: string;
  contentHtml: string;
  meaningVi: string;
  orderIndex: number;
}

export interface SpeakingLessonDetail {
  lesson: SpeakingLesson;
  sentences: SpeakingSentence[];
}
