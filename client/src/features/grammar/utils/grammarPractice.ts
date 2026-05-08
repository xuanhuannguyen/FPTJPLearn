import type { GrammarExercise, GrammarExerciseType } from '../types/grammar.types';

export const grammarExerciseTypes: GrammarExerciseType[] = ['vi_to_ja', 'ja_to_vi', 'arrange'];

export const getPracticeInstructions = (type: GrammarExerciseType) => {
  switch (type) {
    case 'arrange':
      return 'Chọn các mảnh câu theo đúng thứ tự để hoàn thành câu tiếng Nhật.';
    case 'ja_to_vi':
      return 'Đọc câu tiếng Nhật, nhập nghĩa tiếng Việt rồi kiểm tra với đáp án mẫu.';
    default:
      return 'Đọc câu tiếng Việt, nhập bản dịch tiếng Nhật rồi kiểm tra với đáp án mẫu.';
  }
};

export const getPromptLabel = (type: GrammarExerciseType) => {
  if (type === 'vi_to_ja') {
    return 'Tiếng Việt';
  }

  if (type === 'arrange') {
    return 'Sắp xếp câu';
  }

  return '日本語';
};

export const getAnswerLabel = (type: GrammarExerciseType) => {
  if (type === 'vi_to_ja') {
    return 'Bản dịch của bạn (日本語)';
  }

  if (type === 'arrange') {
    return 'Thứ tự bạn chọn';
  }

  return 'Bản dịch của bạn (Tiếng Việt)';
};

export const getAnswerPlaceholder = (type: GrammarExerciseType) => {
  return type === 'vi_to_ja' ? 'Gõ bản dịch tiếng Nhật ở đây...' : 'Nhập nghĩa tiếng Việt ở đây...';
};

export const getSelectedOptionOrder = (exercise: GrammarExercise, selectedOptionIndexes: number[]) => {
  const options = exercise.options ?? [];
  return selectedOptionIndexes
    .map((index) => options[index])
    .filter((option): option is string => Boolean(option));
};
