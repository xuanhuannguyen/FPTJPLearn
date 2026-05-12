export interface VocabularyCourse {
  id: string;
  code: string;
  title: string;
  description?: string;
  lessonCount: number;
  wordCount: number;
  learnedCount: number;
  practicedCount: number;
}

export interface StaticVocabularyLesson {
  id: string;
  courseCode: string;
  lessonNumber: number;
  title: string;
  description?: string;
  accessTier: string;
  packageCode?: string;
  isLocked: boolean;
  wordCount: number;
  learnedCount: number;
  practicedCount: number;
}

export interface StaticVocabularyItem {
  id: string;
  lessonId: string;
  courseCode: string;
  word: string;
  reading: string;
  wordType: string;
  meaning: string;
  exampleJapanese?: string;
  exampleReading?: string;
  exampleMeaning?: string;
  notes?: string;
  accessTier: string;
  packageCode?: string;
  isLocked: boolean;
  isLearned: boolean;
  flashcardPracticeCount: number;
  multipleChoicePracticeCount: number;
  typingPracticeCount: number;
  orderIndex: number;
}

export interface StaticVocabularyLessonDetail {
  lesson: StaticVocabularyLesson;
  items: StaticVocabularyItem[];
}

export interface VocabularyMemoryStatus {
  isInMemory: boolean;
  memoryItemId?: string | null;
  isActive: boolean;
}

export interface VocabularyPracticeCard {
  itemId: string;
  mode: string;
  prompt: string;
  promptReading?: string;
  correctAnswer: string;
  options: string[];
  word: string;
  reading: string;
  meaning: string;
  exampleJapanese?: string;
  exampleMeaning?: string;
}
