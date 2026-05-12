import { BrainCircuit, Castle, Crown, type LucideIcon } from 'lucide-react';
import type { MemoryItemType } from './types/memory.types';

export type MemoryTypeConfig = {
  type: MemoryItemType;
  label: string;
  summaryLabel: string;
  emptyText: string;
  reviewPath: string;
  listPath: string;
  icon: LucideIcon;
  canResetProgress: boolean;
  accentClassName: string;
};

export const memoryTypeConfigs: Record<MemoryItemType, MemoryTypeConfig> = {
  kanji: {
    type: 'kanji',
    label: 'Kanji',
    summaryLabel: 'Kanji',
    emptyText: 'Kanji trong Ghi nhớ sẽ được thêm sau khi bạn bấm "Add to memory" ở màn hình học Kanji.',
    reviewPath: '/memory/kanji/review',
    listPath: '/memory/kanji/list',
    icon: Castle,
    canResetProgress: true,
    accentClassName: 'bg-blue-500',
  },
  vocabulary: {
    type: 'vocabulary',
    label: 'Từ vựng',
    summaryLabel: 'Từ vựng',
    emptyText: 'Hãy mở một bài từ vựng và bấm "Ghi nhớ" ở từ bạn muốn học.',
    reviewPath: '/memory/vocabulary/review',
    listPath: '/memory/vocabulary/list',
    icon: Crown,
    canResetProgress: true,
    accentClassName: 'bg-emerald-500',
  },
  grammar: {
    type: 'grammar',
    label: 'Ngữ pháp',
    summaryLabel: 'Thẻ ngữ pháp',
    emptyText: 'Hãy mở một mẫu ngữ pháp và bấm "Thêm vào ghi nhớ".',
    reviewPath: '/memory/grammar/review',
    listPath: '/memory/grammar/list',
    icon: BrainCircuit,
    canResetProgress: true,
    accentClassName: 'bg-violet-500',
  },
};

export const memoryTypeOrder: MemoryItemType[] = ['kanji', 'vocabulary', 'grammar'];

export const getMemoryTypeConfig = (type: MemoryItemType) => memoryTypeConfigs[type];
