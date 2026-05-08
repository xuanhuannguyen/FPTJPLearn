export type ReviewMode = 'flashcard' | 'multichoice' | 'typing';
export type ReviewScope =
  | 'due'
  | 'all'
  | 'mastered'
  | 'reviewed'
  | 'level-0'
  | 'level-1'
  | 'level-2';

export interface ReviewCard {
  id: string;
  itemId: string;
  word: string;
  reading: string;
  wordType: string;
  meaning: string;
  exampleSentence?: string;
  exampleMeaning?: string;
  level: number;
  status: string;
  nextReviewAt: string;
  intervalDays: number;
  repetitions: number;
  lapseCount: number;
  learningStepIndex: number;
}

export interface DueCardsResponse {
  dueCount: number;
  cards: ReviewCard[];
}

export interface ReviewAnswerResult {
  itemId: string;
  oldLevel: number;
  newLevel: number;
  oldStatus: string;
  newStatus: string;
  nextReviewAt: string;
  intervalDays: number;
  repetitions: number;
  lapseCount: number;
  learningStepIndex: number;
  requeueInSession: boolean;
  requeueAfterSeconds?: number | null;
}

export interface ReviewSessionPayload {
  listId: string;
  mode: ReviewMode;
  sessionType: string;
  totalCards: number;
  correctCount: number;
  wrongCount: number;
  durationSeconds: number;
}

export interface ResetProgressPayload {
  resetType: 'all' | 'mastered' | 'selected';
  hardReset?: boolean;
  itemIds?: string[];
}
