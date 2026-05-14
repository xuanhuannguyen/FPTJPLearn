export type MemoryItemType = 'grammar' | 'kanji' | 'vocabulary';
export type MemoryStatus = 'new' | 'learning' | 'review' | 'mastered' | 'relearning';
export type MemoryScope = 'due' | 'all' | 'new' | 'learning' | 'short_term' | 'long_term';

export interface MemoryTypeSummary {
  due: number;
  new: number;
  learning: number;
  shortTerm: number;
  longTerm: number;
  totalStudied: number;
  nextReviewAt?: string | null;
}

export interface MemorySummary {
  kanji: MemoryTypeSummary;
  vocabulary: MemoryTypeSummary;
  grammar: MemoryTypeSummary;
}

export interface MemoryCard {
  id: string;
  itemType: MemoryItemType;
  sourceKanjiItemId?: string | null;
  sourceGrammarPatternId?: string | null;
  sourceVocabularyItemId?: string | null;
  frontPrimary: string;
  frontSecondary?: string;
  frontMeta?: string;
  backPrimary: string;
  backSecondary?: string;
  example?: string;
  exampleReading?: string;
  exampleMeaning?: string;
  notes?: string;
  level: number;
  status: MemoryStatus;
  nextReviewAt: string;
}

export interface MemoryCardsResponse {
  count: number;
  cards: MemoryCard[];
}

export interface AddGrammarToMemoryResult {
  memoryItemId: string;
  sourceGrammarPatternId: string;
  alreadyExists: boolean;
  isActive: boolean;
}

export interface AddKanjiToMemoryResult {
  memoryItemId: string;
  sourceKanjiItemId: string;
  alreadyExists: boolean;
  isActive: boolean;
}

export interface AddVocabularyToMemoryResult {
  memoryItemId: string;
  sourceVocabularyItemId: string;
  alreadyExists: boolean;
  isActive: boolean;
}

export interface MemoryGrammarStatus {
  isInMemory: boolean;
  memoryItemId?: string | null;
  isActive: boolean;
}

export interface MemoryAnswerResult {
  memoryItemId: string;
  itemType: MemoryItemType;
  oldLevel: number;
  level: number;
  oldStatus: MemoryStatus;
  status: MemoryStatus;
  intervalMinutes: number;
  intervalDays: number;
  nextReviewAt: string;
  repetitions: number;
  lapseCount: number;
  requeueInSession: boolean;
  requeueAfterSeconds?: number;
  message: string;
}

export interface ResetMemoryResult {
  success: boolean;
  count: number;
}
