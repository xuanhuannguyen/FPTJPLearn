export interface ExamCourse {
  id: string;
  code: string;
  title: string;
  description?: string | null;
  accessTier: string;
  packageCode?: string | null;
  isLocked: boolean;
  questionCount: number;
  passageCount: number;
}

export interface ExamTopic {
  topic: string;
  label: string;
  questionCount: number;
}

export interface ExamPassage {
  id: string;
  courseCode: string;
  title: string;
  content: string;
  level: string;
  topic: string;
}

export interface ExamQuestionOption {
  id: string;
  label: string;
  text: string;
}

export interface ExamQuestion {
  id: string;
  courseCode: string;
  questionType: string;
  topic: string;
  level: string;
  questionText: string;
  passageId?: string | null;
}

export interface ExamQuestionDetail extends ExamQuestion {
  passage?: ExamPassage | null;
  options: ExamQuestionOption[];
}

export interface ExamAnswerResult {
  questionId: string;
  selectedOptionId: string;
  correctOptionId: string;
  isCorrect: boolean;
  explanation: string;
}

export interface StartExamAttemptRequest {
  courseCode?: string;
  level?: string;
  topics?: string[];
  questionCount?: number;
  durationMinutes?: number;
  mode?: string;
}

export interface ExamAttemptQuestion extends ExamQuestionDetail {
  attemptAnswerId: string;
  selectedOptionId?: string | null;
  isCorrect?: boolean | null;
  sequenceNumber: number;
}

export interface ExamAttempt {
  id: string;
  courseCode: string;
  mode: string;
  status: string;
  startedAt: string;
  expiresAt: string;
  submittedAt?: string | null;
  durationMinutes: number;
  totalQuestions: number;
  correctCount: number;
  scorePercent: number;
  questions: ExamAttemptQuestion[];
}

export interface ExamAttemptAnswer {
  id: string;
  questionId: string;
  selectedOptionId?: string | null;
  isCorrect?: boolean | null;
  answeredAt?: string | null;
  sequenceNumber: number;
}

export interface ExamReviewOption extends ExamQuestionOption {
  isCorrect: boolean;
}

export interface ExamAttemptReviewQuestion extends ExamQuestion {
  passage?: ExamPassage | null;
  options: ExamReviewOption[];
  selectedOptionId?: string | null;
  correctOptionId: string;
  isCorrect: boolean;
  explanation: string;
  sequenceNumber: number;
}

export interface ExamAttemptReview {
  id: string;
  courseCode: string;
  mode: string;
  status: string;
  startedAt: string;
  submittedAt?: string | null;
  totalQuestions: number;
  correctCount: number;
  scorePercent: number;
  questions: ExamAttemptReviewQuestion[];
}
