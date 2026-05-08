import type { GrammarProgress } from '../types/grammar.types';

export const levelMeta = {
  N5: {
    title: 'Beginner',
    subtitle: 'Nền tảng câu cơ bản, chỉ thị, sở hữu và hành động thường ngày.',
    color: 'bg-emerald-100 text-emerald-700',
  },
  N4: {
    title: 'Elementary',
    subtitle: 'Mẫu câu giao tiếp hằng ngày và diễn đạt ý dài hơn.',
    color: 'bg-blue-100 text-blue-700',
  },
  N3: {
    title: 'Intermediate',
    subtitle: 'Liên kết ý, sắc thái và cách nói tự nhiên hơn.',
    color: 'bg-amber-100 text-amber-700',
  },
  N2: {
    title: 'Advanced',
    subtitle: 'Ngữ pháp học thuật, công việc và văn viết.',
    color: 'bg-rose-100 text-rose-700',
  },
  N1: {
    title: 'Mastery',
    subtitle: 'Cấu trúc trừu tượng, văn chương và sắc thái chuyên sâu.',
    color: 'bg-slate-100 text-slate-700',
  },
};

export const getStudyLevelLabel = (level?: number) => {
  if (level == null) {
    return 'Not studying';
  }

  const safeLevel = Math.min(Math.max(level, 0), 3);
  return safeLevel >= 3 ? 'Mastered' : `Level ${safeLevel}`;
};

export const getStatusClass = (status?: GrammarProgress['status']) => {
  switch (status) {
    case 'mastered':
      return 'bg-accent-success/10 text-accent-success border-accent-success/20';
    case 'review':
      return 'bg-sky-100 text-sky-700 border-sky-200';
    case 'learning':
    case 'relearning':
      return 'bg-accent-warning/10 text-accent-warning border-accent-warning/20';
    case 'new':
      return 'bg-accent-info/10 text-accent-info border-accent-info/20';
    default:
      return 'bg-bg-tertiary text-text-muted border-border/40';
  }
};

export const getExerciseTitle = (type: string) => {
  switch (type) {
    case 'vi_to_ja':
      return 'Việt -> Nhật';
    case 'ja_to_vi':
      return 'Nhật -> Việt';
    case 'arrange':
      return 'Sắp xếp';
    default:
      return type;
  }
};
