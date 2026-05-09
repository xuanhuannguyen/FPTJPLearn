export type KanjiLevel = 'N5' | 'N4' | 'N3' | 'N2' | 'N1';

export interface KanjiLesson {
  id: string;
  level: KanjiLevel;
  lessonNumber: number;
  title: string;
  description?: string;
  accessTier?: string;
  packageCode?: string;
  orderIndex: number;
  kanjiCount?: number;
  vocabularyCount?: number;
  isLocked?: boolean;
}

export interface KanjiComponent {
  character: string;
  name: string;
  kanjiId?: string;
  reading?: string;
}

export interface KanjiItem {
  id: string;
  lessonId: string;
  level: KanjiLevel;
  character: string;
  hanViet: string;
  meaning: string;
  strokeCount: number;
  kunReading: string;
  onReading: string;
  mnemonic: string;
  strokeSvg?: string;
  strokeDataJson?: string;
  componentMapJson?: string;
  components?: KanjiComponent[];
}

export interface KanjiVocabulary {
  id: string;
  lessonId: string;
  kanjiItemId?: string;
  level: KanjiLevel;
  word: string;
  reading: string;
  meaning: string;
  exampleJapanese?: string;
  exampleReading?: string;
  exampleMeaning?: string;
}

export interface KanjiLevelStats {
  level: KanjiLevel;
  totalLessons: number;
  totalKanji: number;
  totalVocabulary: number;
  progressPercentage?: number;
}
