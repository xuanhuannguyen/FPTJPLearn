import type { AdminExamQuestion, AdminExamQuestionPayload } from '../types/adminExam.types';

export const COURSE_OPTIONS = [
  { value: 'jpd113', label: 'JPD113' },
  { value: 'jpd123', label: 'JPD123' },
];

export const TOPIC_OPTIONS = [
  { value: 'kanji', label: 'Kanji' },
  { value: 'grammar', label: 'Ngữ pháp' },
  { value: 'vocabulary', label: 'Từ vựng' },
  { value: 'odd_one_out', label: 'Khác loại' },
  { value: 'reading', label: 'Đọc hiểu' },
];

export const TYPE_OPTIONS = [
  { value: 'standalone', label: 'Câu hỏi thường' },
  { value: 'passage', label: 'Đọc đoạn văn' },
];

export const LEVEL_OPTIONS = ['N5', 'N4', 'N3', 'N2', 'N1'];
export const OPTION_LABELS = ['A', 'B', 'C', 'D'];

export type OptionForm = {
  id?: string;
  label: string;
  text: string;
  isCorrect: boolean;
};

export type QuestionForm = {
  id?: string;
  courseCode: string;
  questionType: string;
  topic: string;
  level: string;
  questionText: string;
  explanation: string;
  passageId?: string | null;
  passageTitle: string;
  passageContent: string;
  orderIndex: string;
  isActive: boolean;
  options: OptionForm[];
};

export const emptyQuestionForm = (): QuestionForm => ({
  courseCode: 'jpd113',
  questionType: 'standalone',
  topic: 'vocabulary',
  level: 'N5',
  questionText: '',
  explanation: '',
  passageId: null,
  passageTitle: '',
  passageContent: '',
  orderIndex: '',
  isActive: true,
  options: OPTION_LABELS.map((label, index) => ({
    label,
    text: '',
    isCorrect: index === 0,
  })),
});

export const toQuestionForm = (question: AdminExamQuestion): QuestionForm => ({
  id: question.id,
  courseCode: question.courseCode,
  questionType: question.questionType,
  topic: question.topic,
  level: question.level,
  questionText: question.questionText,
  explanation: question.explanation,
  passageId: question.passageId ?? null,
  passageTitle: question.passage?.title ?? '',
  passageContent: question.passage?.content ?? '',
  orderIndex: String(question.orderIndex),
  isActive: question.isActive,
  options: OPTION_LABELS.map((label, index) => {
    const option = question.options.find((item) => item.label === label) ?? question.options[index];
    return {
      id: option?.id,
      label,
      text: option?.text ?? '',
      isCorrect: option?.isCorrect ?? index === 0,
    };
  }),
});

export const toQuestionPayload = (form: QuestionForm): AdminExamQuestionPayload => {
  const isPassage = form.questionType === 'passage';

  return {
    courseCode: form.courseCode,
    questionType: form.questionType,
    topic: form.topic,
    level: form.level,
    questionText: form.questionText,
    explanation: form.explanation,
    passageId: isPassage ? form.passageId ?? null : null,
    passage: isPassage
      ? {
          title: form.passageTitle,
          content: form.passageContent,
          level: form.level,
          topic: 'reading',
        }
      : null,
    options: form.options.map((option) => ({
      id: option.id,
      label: option.label,
      text: option.text,
      isCorrect: option.isCorrect,
    })),
    orderIndex: form.orderIndex ? Number(form.orderIndex) : null,
    isActive: form.isActive,
  };
};
