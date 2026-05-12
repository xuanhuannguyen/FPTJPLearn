export interface AdminExamPassage {
  id: string;
  courseCode: string;
  title: string;
  content: string;
  level: string;
  topic: string;
  orderIndex: number;
  isActive: boolean;
}

export interface AdminExamQuestionOption {
  id: string;
  label: string;
  text: string;
  isCorrect: boolean;
  orderIndex: number;
}

export interface AdminExamQuestion {
  id: string;
  courseCode: string;
  questionType: string;
  topic: string;
  level: string;
  questionText: string;
  explanation: string;
  passageId?: string | null;
  passage?: AdminExamPassage | null;
  options: AdminExamQuestionOption[];
  orderIndex: number;
  isActive: boolean;
}

export interface AdminExamQuestionPayload {
  courseCode: string;
  questionType: string;
  topic: string;
  level: string;
  questionText: string;
  explanation: string;
  passageId?: string | null;
  passage?: {
    title: string;
    content: string;
    level?: string | null;
    topic?: string | null;
    orderIndex?: number | null;
  } | null;
  options: Array<{
    id?: string;
    label?: string;
    text: string;
    isCorrect: boolean;
  }>;
  orderIndex?: number | null;
  isActive?: boolean;
}

export interface AdminExamImportResult {
  importedPassages: number;
  createdQuestions: number;
  updatedQuestions: number;
  totalQuestions: number;
}
