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
  lessonType: string;
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

export interface QaQuestion {
  questionId: string;
  order: number;
  question: {
    ja: string;
    vi: string;
  };
  answerType: string;
  sampleAnswers: Array<{
    ja: string;
    vi: string;
  }>;
  grammarIds: string[];
  relatedVocabulary: Array<{
    word: string;
    reading?: string;
    meaning: string;
  }>;
  tips: string[];
  commonMistakes: string[];
  explanation?: string;
  relatedGrammar?: Array<{
    pattern: string;
    meaning: string;
    howToUse: string;
  }>;
}

export interface QaSection {
  sectionId: string;
  sectionTitle: string;
  sectionViTitle: string;
  sectionGoal: string;
  questionList: QaQuestion[];
}

export interface QaVocabularyItem {
  word: string;
  reading?: string;
  kanji?: string;
  meaning: string;
  type?: string;
  note?: string;
  number?: number;
  examples?: Array<{
    ja: string;
    vi: string;
  }>;
}

export interface QaVocabularySet {
  setId: string;
  title: string;
  items: QaVocabularyItem[];
}

export interface QaGrammarItem {
  grammarId: string;
  pattern: string;
  meaning: string;
  usage: string;
  example: {
    ja: string;
    vi: string;
  };
}

export interface PictureSet {
  pictureId: string;
  pictureTitle: string;
  imageUrl: string;
  questions: QaQuestion[];
}

export interface QaLessonDetail {
  courseCode: string;
  lessonNumber: number;
  lessonTitle: string;
  questionMode: 'NO_IMAGE' | 'WITH_IMAGE';
  dataPurpose: string;
  lessonOverview: {
    shortSummary: string;
    studentCanDo: string[];
    mainSkills: string[];
    mainGrammarFocus: string[];
    examTipSummary: string;
  };
  grammarBank?: QaGrammarItem[];
  vocabularySets?: QaVocabularySet[];
  sections?: QaSection[];
  pictureSets?: PictureSet[];
}
