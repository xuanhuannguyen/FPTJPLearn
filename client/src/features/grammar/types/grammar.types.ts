export type GrammarLevel = 'N5' | 'N4' | 'N3' | 'N2' | 'N1';

export interface GrammarLevelSummary {
  level: GrammarLevel;
  courseCode: string;
  lessonCount: number;
  patternCount: number;
  freeCount: number;
  premiumCount: number;
  inStudyCount: number;
  masteredCount: number;
  dueCount: number;
}

export interface GrammarLesson {
  id: string;
  level: GrammarLevel;
  courseCode: string;
  lessonNumber: number;
  title: string;
  description?: string;
  patternCount: number;
  accessTier: 'free' | 'premium';
  packageCode?: string;
  isLocked: boolean;
  inStudyCount: number;
  masteredCount: number;
  dueCount: number;
}

export interface GrammarPattern {
  id: string;
  lessonId: string;
  level: GrammarLevel;
  courseCode: string;
  pattern: string;
  title: string;
  meaning: string;
  structure: string;
  accessTier: 'free' | 'premium';
  packageCode?: string;
  isLocked: boolean;
  isInStudy: boolean;
  progress?: GrammarProgress | null;
  usageScope?: string;
  formation?: string;
  notes?: string;
  tagsJson?: string;
  examples?: GrammarExample[];
  exercises?: GrammarExercise[];
}

export interface GrammarExample {
  id: string;
  japanese: string;
  reading?: string;
  meaning: string;
  note?: string;
  orderIndex: number;
}

export type GrammarExerciseType = 'vi_to_ja' | 'ja_to_vi' | 'arrange';

export interface GrammarExercise {
  id: string;
  patternId: string;
  exerciseType: GrammarExerciseType;
  prompt: string;
  promptReading?: string;
  expectedAnswer?: string;
  hint?: string;
  explanation?: string;
  templateText?: string;
  optionsJson?: string;
  options?: string[];
  starPosition?: number;
  orderIndex: number;
}

export interface GrammarExerciseAnswer extends GrammarExercise {
  expectedAnswer: string;
  acceptableAnswersJson?: string;
  acceptableAnswers: string[];
  correctOrderJson?: string;
  correctOrder: string[];
  starAnswer?: string;
}

export interface GrammarProgress {
  id: string;
  patternId: string;
  level: number;
  status: 'new' | 'learning' | 'review' | 'mastered' | 'relearning';
  nextReviewAt: string;
  intervalDays: number;
  repetitions: number;
  lapseCount: number;
  isActive: boolean;
}

export interface GrammarProgressSummary {
  inStudyCount: number;
  dueCount: number;
  masteredCount: number;
  learningCount: number;
  reviewCount: number;
}

export interface GrammarReviewCard {
  progressId: string;
  patternId: string;
  level: GrammarLevel;
  pattern: string;
  title: string;
  meaning: string;
  structure: string;
  usageScope?: string;
  formation?: string;
  notes?: string;
  examples: GrammarExample[];
  studyLevel: number;
  status: GrammarProgress['status'];
  nextReviewAt: string;
  intervalDays: number;
  repetitions: number;
}

export interface GrammarDueResponse {
  dueCount: number;
  cards: GrammarReviewCard[];
}

export interface AddGrammarStudyResult {
  success: boolean;
  alreadyExists: boolean;
  progress?: GrammarProgress;
}

export interface GrammarAnswerResult {
  patternId: string;
  oldLevel: number;
  newLevel: number;
  oldStatus: GrammarProgress['status'];
  newStatus: GrammarProgress['status'];
  nextReviewAt: string;
  intervalDays: number;
  repetitions: number;
  lapseCount: number;
  learningStepIndex: number;
  requeueInSession: boolean;
  requeueAfterSeconds?: number;
}

export interface GrammarExerciseCheckResult {
  exerciseId: string;
  isCorrect: boolean;
  score: number;
  feedback: string;
  expectedAnswer: string;
  correctOrderJson?: string;
  starAnswer?: string;
  attemptId: string;
}
